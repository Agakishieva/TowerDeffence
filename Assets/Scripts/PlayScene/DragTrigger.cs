using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragTrigger : EventTrigger
{
    private bool dragging;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (dragging == true)
        {
            StartCoroutine(CardAnimation());
        }
    }

    public IEnumerator CardAnimation()
    {
        animator.SetTrigger("select");
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("death");
             
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        //Cursor.visible = true;  for testing on mobile
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        //Cursor.visible = false;  for testing on mobile
    }

}
