using Utility.ObjPool;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Utility;
using System.Collections;

public class RendererGetter : MonoBehaviour
{
    InstantPool<Image> imagePool;
    InstantPool<TMP_Text> textPool;
    InstantPool<SpriteRenderer> spritePool;
    [SerializeField] Transform spritePoolParent;

    [SerializeField] TMP_Text rawTextPref;
    [SerializeField] Image rawImageObj;
    [SerializeField] SpriteRenderer rendererObj;

    void Start()
    {
        GameManager.instance.onGameEvent += (x) =>
        {
            if (x == GameState.serverInitialize)
            {
                StartCoroutine(Initialize());
            }
        };
    }

    IEnumerator Initialize()
    {
        var task = new InteraptTask(GameManager.instance);

        imagePool = new InstantPool<Image>(transform);
        textPool = new InstantPool<TMP_Text>(transform);
        spritePool = new InstantPool<SpriteRenderer>(spritePoolParent);

        textPool.CreatePool(rawTextPref, 10);
        yield return new WaitUntil(()=>textPool.state==ModuleState.ready);
        imagePool.CreatePool(rawImageObj, 10);
        yield return new WaitUntil(()=>imagePool.state==ModuleState.ready);
        spritePool.CreatePool(rendererObj, 10);
        yield return new WaitUntil(()=>spritePool.state==ModuleState.ready);
        
        task.finished = true;
    }

    public SpriteRenderer GetSpriteRendererObj()
    {
        return spritePool.GetObj();
    }

    public Image GetImageObj()
    {
        return imagePool.GetObj();
    }

    public TMP_Text GetTextObj()
    {
        return textPool.GetObj();
    }
}