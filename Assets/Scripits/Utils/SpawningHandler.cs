using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawningHandler
{

    public static GameObject SpawnEntity(Sprite EntitySprite, RuntimeAnimatorController animatorController, Sprite selectionSprite = null, Sprite HPSprite = null)
    {
        GameObject human = new GameObject("Hooman");
        human.transform.parent = GameObject.Find("Settlers").transform;
        human.transform.position = new Vector3(0, 0);
        human.AddComponent<SpriteRenderer>();
        human.AddComponent<Animator>();
        human.GetComponent<SpriteRenderer>().sprite = EntitySprite;
        human.AddComponent<Pathfinding>();
        human.AddComponent<MoveTransformVelocity>();
        human.GetComponent<MoveTransformVelocity>().moveSpeed = 2;
        human.AddComponent<MovePositionNodes>();
        human.AddComponent<BoxCollider2D>();
        human.GetComponent<BoxCollider2D>().size = new Vector2(4, 5);
        human.GetComponent<Animator>().runtimeAnimatorController = animatorController;

        if (selectionSprite == null) return human;

        GameObject selected = new GameObject("Selected");
        selected.transform.parent = human.transform;
        selected.AddComponent<SpriteRenderer>();
        selected.GetComponent<SpriteRenderer>().sprite = selectionSprite;
        selected.transform.localScale = new Vector3(2, 2);
        selected.transform.position = new Vector3(0, (float)-2.4);
        CreateHPbar(selected, HPSprite);
        human.AddComponent<UnitRTS>();
        return human;
    }

    public static void CreateHPbar(GameObject parent, Sprite HPBackgroundSprite)
    {
        GameObject hpBar = new GameObject("HpBar");
        GameObject hpBackground = new GameObject("HpBackground");
        GameObject hp = new GameObject("Hp");

        hpBackground.transform.parent = hpBar.transform;
        hp.transform.parent = hpBar.transform;

        hpBackground.AddComponent<SpriteRenderer>();
        hp.AddComponent<SpriteRenderer>();

        hpBackground.GetComponent<SpriteRenderer>().sprite = HPBackgroundSprite;
        hp.GetComponent<SpriteRenderer>().sprite = HPBackgroundSprite;

        hpBackground.GetComponent<SpriteRenderer>().color = new Color(255, 120, 120);
        hp.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);

        //hpBackground.GetComponent<SpriteRenderer>().spriteSortPoint = SpriteSortPoint.Pivot;
        hp.GetComponent<SpriteRenderer>().sortingOrder = 1;

        hpBackground.transform.localScale = new Vector3(1, .2f, 1);
        hp.transform.localScale = new Vector3(1, .2f, 1);

        hpBackground.transform.position = new Vector3(-.5f, 1.5f, 0);
        hp.transform.position = new Vector3(-.5f, 1.5f, 1);

        hpBar.transform.parent = (parent.transform);
        hpBar.transform.position = parent.transform.position;
        hpBar.transform.localScale = parent.transform.localScale;
    }
}
