using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //config params
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] Sprite[] hitSprites;
    
    //cached reference
    Level level;
    GameSession gameStats;

    //state variables
    [SerializeField] int timesHit;

    private void Start()
    {
        CountBreakableBlocks();

    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if(tag == "Breakable")
        {
            HandleHit();

        }
        else if(tag == "Unbreakable")
        {
            Debug.Log("ITS UNBREAKABLE!!!!!");
        }

    }

    private void HandleHit()
    {
        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit == maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if(hitSprites[spriteIndex] != null)
        {

            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];

        }
        else
        {
            Debug.Log("Block sprite is missing from array" + gameObject.name);
        }
        
    }

    private void DestroyBlock()
    {
        
        Destroy(gameObject);
        PlayDestroySFX();
        level.BlockDestroyed();
        gameStats.AddToScore();
        TriggerSparklesVFX();
    }

    private void PlayDestroySFX()
    {
        gameStats = FindObjectOfType<GameSession>();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void TriggerSparklesVFX()
    {

        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 2f);
       
    }
}
