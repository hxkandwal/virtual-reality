using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour {
	public PathNode m_parent;

	//子路点
	public PathNode m_next;

	// Use this for initialization
	//设置当前路点的子路点函数

	public void SetNext(PathNode node) {

		if (m_next != null) {
			m_next.m_parent=null;
		}

		m_next = node;
		node.m_parent = this;
	}
	
	void OnDrawGizmos() {
	//	Gizmos.DrawIcon (this.transform.position,"hbs01tif.tif");
	}

}
