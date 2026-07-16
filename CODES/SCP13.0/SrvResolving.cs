using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using DnsClient;
using DnsClient.Data;
using DnsClient.Data.Records;
using DnsClient.Logging;
using DnsClient.Enums; // Добавлено для QType и DnsErrorCode
using UnityEngine; // Добавлено для Color

internal static class SrvResolving
{
    private const ushort DnsMaxAttempts = 3;
    private const ushort DnsTimeout = 350;

    private static IPEndPoint _dnsEndpoint;
    private static DnsConsoleLogger _dnsLogger;

    internal static async Task<DnsRecord.SRVRecord> ResolveSRV(string domain)
    {
        try
        {
            if (_dnsEndpoint == null)
            {
                IPAddress dnsIp = FindDnsServer("1.1.1.1");
                _dnsEndpoint = new IPEndPoint(dnsIp, 53);
                _dnsLogger = new DnsConsoleLogger();

                GameCore.Console.AddLog($"[DNS CLIENT] Using DNS server: {_dnsEndpoint}", Color.cyan);
            }

            var options = new DnsClientOptions
            {
                MaxAttempts = DnsMaxAttempts,
                ErrorLogging = _dnsLogger
            };

            var client = new DnsClient.DnsClient(_dnsEndpoint, options);

            DnsResponse response = await client.Query("_scpsl._udp." + domain, QType.SRV);

            client.Dispose();

            if (response == null || response.Records == null || response.Records.Count == 0)
                return null;

            var srvRecords = response.Records
                .Where(r => r.Type == QType.SRV)
                .Cast<DnsRecord.SRVRecord>()
                .ToList();

            if (srvRecords.Count == 0) return null;

            int totalWeight = srvRecords.Sum(r => (int)r.Weight);
            int randomWeight = new System.Random().Next(0, totalWeight + 1);

            return SelectRecord(srvRecords, randomWeight);
        }
        catch (Exception e)
        {
            _dnsLogger?.LogException($"Failed to resolve SRV for {domain}", e);
            return null;
        }
    }

    private static DnsRecord.SRVRecord SelectRecord(IReadOnlyList<DnsRecord.DNSRecord> records, int randomWeight)
    {
        var srvRecords = records.OfType<DnsRecord.SRVRecord>().OrderBy(r => r.Priority).ToList();
        if (srvRecords.Count == 0) return null;

        int currentWeightSum = 0;
        foreach (var record in srvRecords)
        {
            currentWeightSum += record.Weight;
            if (currentWeightSum >= randomWeight)
            {
                return record;
            }
        }

        GameCore.Console.AddLog($"[DNS CLIENT] Random weight picked ({randomWeight}) is higher than total sum! Selecting first.", Color.yellow);
        return srvRecords[0];
    }

    private static IPAddress FindDnsServer(string defaultServer)
    {
        try
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up) continue;
                var props = ni.GetIPProperties();
                if (props.DnsAddresses.Count > 0) return props.DnsAddresses[0];
            }
        }
        catch { }
        return IPAddress.Parse(defaultServer);
    }

    private class DnsConsoleLogger : IErrorLogging
    {
        public void LogError(string message)
        {
            GameCore.Console.AddLog($"[DNS CLIENT] Error: {message}", Color.red, false, GameCore.Console.ConsoleLogType.Error);
        }

        public void LogException(string message, Exception e)
        {
            string formatted = string.Format("[DNS CLIENT] Error: {0}{1}Exception: {2} - {3}{4}{5}",
                message, Environment.NewLine, e.GetType(), e.Message, Environment.NewLine, e.StackTrace);

            GameCore.Console.AddLog(formatted, Color.red, false, GameCore.Console.ConsoleLogType.Error);
        }
    }
}