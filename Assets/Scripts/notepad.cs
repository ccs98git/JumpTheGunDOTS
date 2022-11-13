/*
		for (int i = 0; i < 35; i++) {
			int height = Random.Range(4,12);
			Vector3 pos = new Vector3(Random.Range(-45f,45f),0f,Random.Range(-45f,45f));
			float spacing = 2f;
			for (int j = 0; j < height; j++) {
				Point point = new Point();
				point.x = pos.x+spacing;
				point.y = j * spacing;
				point.z = pos.z-spacing;
				point.oldX = point.x;
				point.oldY = point.y;
				point.oldZ = point.z;
				if (j==0) {
					point.anchor=true;
				}
				pointsList.Add(point);
				point = new Point();
				point.x = pos.x-spacing;
				point.y = j * spacing;
				point.z = pos.z-spacing;
				point.oldX = point.x;
				point.oldY = point.y;
				point.oldZ = point.z;
				if (j==0) {
					point.anchor=true;
				}
				pointsList.Add(point);
				point = new Point();
				point.x = pos.x+0f;
				point.y = j * spacing;
				point.z = pos.z+spacing;
				point.oldX = point.x;
				point.oldY = point.y;
				point.oldZ = point.z;
				if (j==0) {
					point.anchor=true;
				}
				pointsList.Add(point);
			}
		}
 
 
 
 
	public void AssignPoints(Point a, Point b) {
		point1 = a;
		point2 = b;
		Vector3 delta = new Vector3(point2.x - point1.x,point2.y - point1.y,point2.z - point1.z);
		length = delta.magnitude;

		thickness = Random.Range(.25f,.35f);

		Vector3 pos = new Vector3(point1.x + point2.x,point1.y + point2.y,point1.z + point2.z) * .5f;
		Quaternion rot = Quaternion.LookRotation(delta);
		Vector3 scale = new Vector3(thickness,thickness,length);
		matrix = Matrix4x4.TRS(pos,rot,scale);

		minX = Mathf.Min(point1.x,point2.x);
		maxX = Mathf.Max(point1.x,point2.x);
		minY = Mathf.Min(point1.y,point2.y);
		maxY = Mathf.Max(point1.y,point2.y);
		minZ = Mathf.Min(point1.z,point2.z);
		maxZ = Mathf.Max(point1.z,point2.z);

		float upDot = Mathf.Acos(Mathf.Abs(Vector3.Dot(Vector3.up,delta.normalized)))/Mathf.PI;
		color = Color.white * upDot*Random.Range(.7f,1f);
	}
 
 
 
 */