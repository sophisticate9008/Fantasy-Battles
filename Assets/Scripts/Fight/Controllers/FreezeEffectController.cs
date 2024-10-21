
using System.Collections;
using System.Collections.Generic;
using MyBase;
using UnityEngine;

public class FreezeEffectController : EffectControllerBase
{
    public Sprite[] sprites;
    public SpriteRenderer bulkSr;
    public float baseHeight;
    private SpriteRenderer enemySr;
    public SpriteRenderer iceSr;
    private SpriteMask spriteMask;
    private float percent = 1;
    public float animeTime = 0.1f;

    public override void Init()
    {
        base.Init();
        transform.SetParent(Enemy.transform);
        transform.position = Enemy.transform.position + new Vector3(0, 0, -0.01f);
        bulkSr.transform.position = Enemy.transform.position;
        enemySr = Enemy.GetComponent<SpriteRenderer>();
        spriteMask = GetComponent<SpriteMask>();
        ChangeScale();
        Align();
    }
    private void ChangeScale()
    {
        float srHeight = enemySr.sprite.bounds.size.y;
        float s = srHeight / baseHeight;
        iceSr.transform.localScale = Vector3.one * s;
        bulkSr.transform.localScale = Vector3.one * s;
    }
    private void Align()
    {
        Sprite enemySprite = enemySr.sprite;
        float pivotToBottom = -enemySprite.pivot.y / enemySprite.pixelsPerUnit;
        Vector2 icePos = enemySr.transform.position;
        iceSr.transform.position = icePos;
    }
    private void LateUpdate()
    {
        if (Enemy)
        {
            spriteMask.sprite = enemySr.sprite;
            int n = Mathf.FloorToInt(percent * sprites.Length);
            if (n > 0)
            {
                iceSr.sprite = sprites[n - 1];
            }
            else
            {
                iceSr.sprite = null;
            }
            if (percent == 1 && !bulkSr.gameObject.activeSelf)
            {
                bulkSr.gameObject.SetActive(true);
            }
            else if (percent < 1 && bulkSr.gameObject.activeSelf)
            {
                bulkSr.gameObject.SetActive(false);
            }
        }
    }

    public override void Play()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        IsPlaying = true;
        // StopAllCoroutines();
        // StartCoroutine(Anime(1));
    }
    public override void Stop()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        // StopAllCoroutines();
        // StartCoroutine(Anime(0));
        IsPlaying = false;
    }
    private IEnumerator Anime(float targetPresent)
    {
        float speed = 1 / animeTime;
        while (percent != targetPresent)
        {
            yield return null;
            percent = Mathf.MoveTowards(percent, targetPresent, speed * Time.deltaTime);
        }
    }


}