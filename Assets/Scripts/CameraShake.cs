using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private float shakeTime;
    private float shakeIntensity;
    [SerializeField] private float power;

    private CameraController cameraController;

    Quaternion m_originRot;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();

        if (null == instance)
        {
            instance = this;
        }

        else
        {
            Destroy(this.gameObject);
        }

    }

    public void OnShakeCamera(float shakeTime = 0.1f, float shakeIntensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakeByRotation");
        StartCoroutine("ShakeByRotation");
    }
   
    IEnumerator ShakeByRotation()
    {
        // 카메라 흔들림 효과 재생 시작
        cameraController.isOnShake = true;

        // 흔들리기 직전의 초기 회전 값
        Vector3 startRotation = transform.eulerAngles;

        while(shakeTime > 0f)
        {
            // 회전하길 원하는 축만 지정해서 사용 (회전하지 않을 축은 0으로 설정)
            // (클래스 멤버변수로 선언해 외부에서 조작하면 더 좋다)
            float x = 0f;
            float y = 0f;
            float z = Random.Range(-1f, 1f);
            transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shakeIntensity * power);

            shakeTime -= Time.deltaTime;

            yield return null;
        
        }

        // 흔들리기 직전의 회전 값으로 설정
        transform.rotation = Quaternion.Euler(startRotation);

        // 카메라 흔들림 효과 재생 종료
        cameraController.isOnShake = false;

    }

}
