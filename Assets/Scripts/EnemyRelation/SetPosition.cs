using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetPosition : MonoBehaviour
{
	//初期位置
	private Vector3 startPosition;
	//目的地
	private Vector3 destination;
	//一度だけ着地時に初期位置設定用
	bool firstGround = false;
	//自身のnavmesh
	NavMeshAgent navMeshAgent = null;
	[SerializeField,Tooltip("ランダムで移動する範囲"),Range(5,20)]
	int createRandomRange = 10;
	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
	}
	//　ランダムな位置の作成
	public void CreateRandomPosition()
	{
		//　ランダムなVector2の値を得る
		var randDestination = Random.insideUnitCircle * createRandomRange;
		//　現在地にランダムな位置を足して目的地とする
		SetDestination(startPosition + new Vector3(randDestination.x, transform.position.y, randDestination.y));
	}

	//　目的地を設定する
	public void SetDestination(Vector3 position)
	{
		destination = position;
	}

	//　目的地を取得する
	public Vector3 GetDestination()
	{
		return destination;
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (!MonobitEngine.MonobitNetwork.isHost)
		{
			return;
		}
		if (firstGround==false)
		{
			if(collision.gameObject.tag=="Ground")
			{
				startPosition = transform.position;
				CreateRandomPosition();
				navMeshAgent.SetDestination(destination);
				firstGround = true;
			}
		}
	}
}
