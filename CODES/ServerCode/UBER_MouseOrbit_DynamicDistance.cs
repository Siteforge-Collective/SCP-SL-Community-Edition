[global::UnityEngine.AddComponentMenu("UBER/Mouse Orbit - Dynamic Distance")]
public class UBER_MouseOrbit_DynamicDistance : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.GameObject target;

	public global::UnityEngine.Transform targetFocus;

	public float distance = 1f;

	[global::UnityEngine.Range(0.1f, 4f)]
	public float ZoomWheelSpeed = 4f;

	public float minDistance = 0.5f;

	public float maxDistance = 10f;

	public float xSpeed = 250f;

	public float ySpeed = 120f;

	public float xObjSpeed = 250f;

	public float yObjSpeed = 120f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	private float x;

	private float y;

	private float normal_angle;

	private float cur_distance;

	private float cur_xSpeed;

	private float cur_ySpeed;

	private float req_xSpeed;

	private float req_ySpeed;

	private float cur_ObjxSpeed;

	private float cur_ObjySpeed;

	private float req_ObjxSpeed;

	private float req_ObjySpeed;

	private bool DraggingObject;

	private bool lastLMBState;

	private global::UnityEngine.Collider[] surfaceColliders;

	private float bounds_MaxSize = 20f;

	[global::UnityEngine.HideInInspector]
	public bool disableSteering;

	private void Start()
	{
		global::UnityEngine.Vector3 eulerAngles = base.transform.eulerAngles;
		x = eulerAngles.y;
		y = eulerAngles.x;
		Reset();
	}

	public void DisableSteering(bool state)
	{
		disableSteering = state;
	}

	public void Reset()
	{
		lastLMBState = global::UnityEngine.Input.GetMouseButton(0);
		disableSteering = false;
		cur_distance = distance;
		cur_xSpeed = 0f;
		cur_ySpeed = 0f;
		req_xSpeed = 0f;
		req_ySpeed = 0f;
		surfaceColliders = null;
		cur_ObjxSpeed = 0f;
		cur_ObjySpeed = 0f;
		req_ObjxSpeed = 0f;
		req_ObjySpeed = 0f;
		if (!target)
		{
			return;
		}
		global::UnityEngine.Renderer[] componentsInChildren = target.GetComponentsInChildren<global::UnityEngine.Renderer>();
		global::UnityEngine.Bounds bounds = default(global::UnityEngine.Bounds);
		bool flag = false;
		global::UnityEngine.Renderer[] array = componentsInChildren;
		foreach (global::UnityEngine.Renderer renderer in array)
		{
			if (!flag)
			{
				flag = true;
				bounds = renderer.bounds;
			}
			else
			{
				bounds.Encapsulate(renderer.bounds);
			}
		}
		global::UnityEngine.Vector3 size = bounds.size;
		float num = ((size.x > size.y) ? size.x : size.y);
		num = ((size.z > num) ? size.z : num);
		bounds_MaxSize = num;
		cur_distance += bounds_MaxSize * 1.2f;
		surfaceColliders = target.GetComponentsInChildren<global::UnityEngine.Collider>();
	}

	private void LateUpdate()
	{
		if (!target || !targetFocus)
		{
			return;
		}
		if (!lastLMBState && global::UnityEngine.Input.GetMouseButton(0))
		{
			DraggingObject = false;
			if (surfaceColliders != null)
			{
				global::UnityEngine.RaycastHit hitInfo = default(global::UnityEngine.RaycastHit);
				global::UnityEngine.Ray ray = global::UnityEngine.Camera.main.ScreenPointToRay(global::UnityEngine.Input.mousePosition);
				global::UnityEngine.Collider[] array = surfaceColliders;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].Raycast(ray, out hitInfo, float.PositiveInfinity))
					{
						DraggingObject = true;
						break;
					}
				}
			}
		}
		else if (lastLMBState && !global::UnityEngine.Input.GetMouseButton(0))
		{
			DraggingObject = false;
		}
		lastLMBState = global::UnityEngine.Input.GetMouseButton(0);
		if (DraggingObject)
		{
			if (global::UnityEngine.Input.GetMouseButton(0) && !disableSteering)
			{
				req_ObjxSpeed += (global::UnityEngine.Input.GetAxis("Mouse X") * xObjSpeed * 0.02f - req_ObjxSpeed) * global::UnityEngine.Time.deltaTime * 10f;
				req_ObjySpeed += (global::UnityEngine.Input.GetAxis("Mouse Y") * yObjSpeed * 0.02f - req_ObjySpeed) * global::UnityEngine.Time.deltaTime * 10f;
			}
			else
			{
				req_ObjxSpeed += (0f - req_ObjxSpeed) * global::UnityEngine.Time.deltaTime * 4f;
				req_ObjySpeed += (0f - req_ObjySpeed) * global::UnityEngine.Time.deltaTime * 4f;
			}
			req_xSpeed += (0f - req_xSpeed) * global::UnityEngine.Time.deltaTime * 4f;
			req_ySpeed += (0f - req_ySpeed) * global::UnityEngine.Time.deltaTime * 4f;
		}
		else
		{
			if (global::UnityEngine.Input.GetMouseButton(0) && !disableSteering)
			{
				req_xSpeed += (global::UnityEngine.Input.GetAxis("Mouse X") * xSpeed * 0.02f - req_xSpeed) * global::UnityEngine.Time.deltaTime * 10f;
				req_ySpeed += (global::UnityEngine.Input.GetAxis("Mouse Y") * ySpeed * 0.02f - req_ySpeed) * global::UnityEngine.Time.deltaTime * 10f;
			}
			else
			{
				req_xSpeed += (0f - req_xSpeed) * global::UnityEngine.Time.deltaTime * 4f;
				req_ySpeed += (0f - req_ySpeed) * global::UnityEngine.Time.deltaTime * 4f;
			}
			req_ObjxSpeed += (0f - req_ObjxSpeed) * global::UnityEngine.Time.deltaTime * 4f;
			req_ObjySpeed += (0f - req_ObjySpeed) * global::UnityEngine.Time.deltaTime * 4f;
		}
		distance -= global::UnityEngine.Input.GetAxis("Mouse ScrollWheel") * ZoomWheelSpeed;
		distance = global::UnityEngine.Mathf.Clamp(distance, minDistance, maxDistance);
		cur_ObjxSpeed += (req_ObjxSpeed - cur_ObjxSpeed) * global::UnityEngine.Time.deltaTime * 20f;
		cur_ObjySpeed += (req_ObjySpeed - cur_ObjySpeed) * global::UnityEngine.Time.deltaTime * 20f;
		target.transform.RotateAround(targetFocus.position, global::UnityEngine.Vector3.Cross(targetFocus.position - base.transform.position, base.transform.right), 0f - cur_ObjxSpeed);
		target.transform.RotateAround(targetFocus.position, global::UnityEngine.Vector3.Cross(targetFocus.position - base.transform.position, base.transform.up), 0f - cur_ObjySpeed);
		cur_xSpeed += (req_xSpeed - cur_xSpeed) * global::UnityEngine.Time.deltaTime * 20f;
		cur_ySpeed += (req_ySpeed - cur_ySpeed) * global::UnityEngine.Time.deltaTime * 20f;
		x += cur_xSpeed;
		y -= cur_ySpeed;
		y = ClampAngle(y, yMinLimit + normal_angle, yMaxLimit + normal_angle);
		if (surfaceColliders != null)
		{
			global::UnityEngine.RaycastHit hitInfo2 = default(global::UnityEngine.RaycastHit);
			global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.Normalize(targetFocus.position - base.transform.position);
			float num = 0.01f;
			bool flag = false;
			global::UnityEngine.Collider[] array = surfaceColliders;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Raycast(new global::UnityEngine.Ray(base.transform.position - vector * bounds_MaxSize, vector), out hitInfo2, float.PositiveInfinity))
				{
					num = global::UnityEngine.Mathf.Max(global::UnityEngine.Vector3.Distance(hitInfo2.point, targetFocus.position) + distance, num);
					flag = true;
				}
			}
			if (flag)
			{
				cur_distance += (num - cur_distance) * global::UnityEngine.Time.deltaTime * 4f;
			}
		}
		global::UnityEngine.Quaternion quaternion = global::UnityEngine.Quaternion.Euler(y, x, 0f);
		global::UnityEngine.Vector3 position = quaternion * new global::UnityEngine.Vector3(0f, 0f, 0f - cur_distance) + targetFocus.position;
		base.transform.rotation = quaternion;
		base.transform.position = position;
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return global::UnityEngine.Mathf.Clamp(angle, min, max);
	}

	public void set_normal_angle(float a)
	{
		normal_angle = a;
	}
}
