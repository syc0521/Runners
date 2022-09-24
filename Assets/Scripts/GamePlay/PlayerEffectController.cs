using Runner.GamePlay;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    public Material leakageMat;
    public ParticleSystem rainbowTrail;

    public float leakageDuration=0.5f;
    public float leakageInterval = 15f;

    private Dictionary<string, Material[]> originMaterialDic;

    public ParticleSystem[] sparks;

    void Start()
    {
        originMaterialDic = new Dictionary<string, Material[]>();
        SaveOriginMaterialInfo();
        //InvokeRepeating("StartLeakageIE", 0f, leakageInterval);
        rainbowTrail.Stop();

        foreach (var v in sparks) v.gameObject.SetActive(false);
    }

    public void StartLeakageIE()
    {
        StartCoroutine(LeakageIE());
    }

    private IEnumerator LeakageIE()
    {
        ChangeMaterial(leakageMat);
        yield return new WaitForSeconds(leakageDuration);
        RestoreMaterial();
    }

    //保存原始材质数据
    private void SaveOriginMaterialInfo()
    {
        foreach (var v in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            originMaterialDic.Add(v.name, v.materials);
        }
    }


    //改变材质
    private void ChangeMaterial(Material mat)
    {
        foreach (var v in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            var tempMat = v.material;
            v.materials = new Material[]
            {
                tempMat,
                leakageMat,
            };
        }
    }

    //恢复材质
    private void RestoreMaterial()
    {
        foreach (var v in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            v.materials = originMaterialDic[v.name];
        }
    }   
}
