using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GameCore;

internal class QueryUser : IDisposable
{
    private readonly NetworkStream _s;

    private readonly QueryServer _server;

    private readonly Thread _thr;

    private readonly Thread _sol;

    private int _lastping;

    private bool _closing;

    private int _invalidPackets;

    private readonly string _querypassword;

    internal readonly string Ip;

    private bool _authenticated;

    private readonly UTF8Encoding _encoder;

    private readonly UserPrint _printer;

    internal ulong Permissions;

    internal byte KickPower;

    internal QueryUser(QueryServer s, TcpClient c, string ip)
    {
        _s = c.GetStream();
        _server = s;
        Ip = ip;
        Send("Welcome to SCP Secret Laboratory Query Protocol");
        _thr = new Thread(Receive)
        {
            IsBackground = true
        };
        _thr.Start();
        _encoder = new UTF8Encoding();
        _querypassword = ConfigFile.ServerConfig.GetString("administrator_query_password");
        _lastping = Convert.ToInt32(_server.Stopwatch.Elapsed.TotalSeconds) + 5;
        _authenticated = false;
        _printer = new UserPrint(this);
    }

    internal bool IsConnected()
    {
        return _server.Stopwatch.Elapsed.TotalSeconds - (double)_lastping < (double)_server.TimeoutThreshold;
    }

    private void Receive()
    {
        _s.ReadTimeout = 200;
        _s.WriteTimeout = 200;
        while (!_closing)
        {
            try
            {
                byte[] array = new byte[4096];
                int num;
                try
                {
                    num = _s.Read(array, 0, 4096);
                }
                catch
                {
                    num = -1;
                    Thread.Sleep(5);
                }
                if (num <= -1)
                {
                    continue;
                }
                foreach (byte[] item in AuthenticatedMessage.Decode(array))
                {
                    string text = _encoder.GetString(item);
                    AuthenticatedMessage authenticatedMessage = null;
                    try
                    {
                        text = text.Substring(0, text.LastIndexOf(';'));
                    }
                    catch
                    {
                        _invalidPackets++;
                        text = text.TrimEnd('\0');
                        if (text.EndsWith(";"))
                        {
                            text = text.Substring(0, text.Length - 1);
                        }
                    }
                    if (_invalidPackets >= 5)
                    {
                        if (!_closing)
                        {
                            Send("Too many invalid packets sent.");
                            ServerConsole.AddLog("Query connection from " + Ip + " dropped due to too many invalid packets sent.");
                            _server.Users.Remove(this);
                            CloseConn();
                        }
                        break;
                    }
                    try
                    {
                        authenticatedMessage = AuthenticatedMessage.AuthenticateMessage(text, TimeBehaviour.CurrentTimestamp(), _querypassword);
                    }
                    catch (MessageAuthenticationFailureException ex)
                    {
                        Send("Message can't be authenticated - " + ex.Message);
                        ServerConsole.AddLog("Query command from " + Ip + " can't be authenticated - " + ex.Message);
                    }
                    catch (MessageExpiredException)
                    {
                        Send("Message expired");
                        ServerConsole.AddLog("Query command from " + Ip + " is expired.");
                    }
                    catch (Exception ex3)
                    {
                        Send("Error during processing your message.");
                        ServerConsole.AddLog("Query command from " + Ip + " can't be processed - " + ex3.Message + ".");
                    }
                    if (authenticatedMessage != null)
                    {
                        if (!_authenticated && authenticatedMessage.Administrator)
                        {
                            _authenticated = true;
                        }
                        if (authenticatedMessage.Message == "Ping")
                        {
                            _invalidPackets = 0;
                            _lastping = Convert.ToInt32(_server.Stopwatch.Elapsed.TotalSeconds);
                            Send("Pong");
                        }
                        else if (AdminCheck(authenticatedMessage.Administrator))
                        {
                            ServerConsole.EnterCommand(authenticatedMessage.Message, _printer);
                        }
                    }
                }
            }
            catch (SocketException)
            {
                ServerConsole.AddLog("Query connection from " + Ip + " dropped (SocketException).");
                if (!_closing)
                {
                    _server.Users.Remove(this);
                    CloseConn();
                }
                break;
            }
            catch
            {
                ServerConsole.AddLog("Query connection from " + Ip + " dropped.");
                if (!_closing)
                {
                    _server.Users.Remove(this);
                    CloseConn();
                }
                break;
            }
        }
    }

    private bool AdminCheck(bool admin)
    {
        if (!admin)
        {
            Send("Access denied! You need to have administrator permissions.");
        }
        return admin;
    }

    public void CloseConn(bool shuttingdown = false)
    {
        _closing = true;
        if (shuttingdown)
        {
            Send("Server is shutting down...");
        }
        _s.Close();
        _thr?.Abort();
        Dispose();
    }

    public void Send(string msg)
    {
        msg = ((!_authenticated || _querypassword == "" || _querypassword == "none" || _querypassword == null) ? AuthenticatedMessage.GenerateNonAuthenticatedMessage(msg) : AuthenticatedMessage.GenerateAuthenticatedMessage(msg, TimeBehaviour.CurrentTimestamp(), _querypassword));
        Send(Utf8.GetBytes(msg));
    }

    public void Send(byte[] msg)
    {
        try
        {
            byte[] array = AuthenticatedMessage.Encode(msg);
            _s.Write(array, 0, array.Length);
        }
        catch (Exception ex)
        {
            ServerConsole.AddLog("Can't send query response to " + Ip + ": " + ex.StackTrace);
        }
    }

    public void Dispose()
    {
        _s?.Dispose();
        if (ServerConsole.ConsoleOutputs.Contains(_printer))
        {
            ServerConsole.ConsoleOutputs.Remove(_printer);
        }
    }
}
