using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//inheriting from Idraghandler ect
//4 states, default, hover, dragging, and play
public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler //added IPointerUpHandler so we can detect release without using old Input system
{
    private RectTransform rectTransform; //basic rect and canvas for our object
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition; //mouse pointer
    private Vector3 originalPanelLocalPosition; //original position of the card
    private Vector3 originalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    private Vector2 latestPointerPosition; //added to store pointer position from the new Input System event data

    [SerializeField] private float selectScale = 1.1f; //scale when card is selected/hovered
    [SerializeField] private Vector2 cardPlay; //if mouse goes past the point, card is played, set in the inspector
    [SerializeField] private Vector3 playPosition;
    //[SerializeField] private GameObject glowEffect; //tbd
    //[SerializeField] private GameObject playArrow; //tbd

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        originalScale = rectTransform.localScale;
        originalRotation = rectTransform.localRotation;
        originalPosition = rectTransform.localPosition;
    }

    void Update()
    {
        switch (currentState)
        {
            case 1: //hoever state
                HandleHoverState();
                break;
            case 2: //dragging state
                HandleDragState();
                break; //removed old Input.GetMouseButton check because release is now handled in OnPointerUp
            case 3: //play state
                HandlePlayState();
                break; //removed old Input.GetMouseButton check because release is now handled in OnPointerUp

        }
    }

    private void TransitionToState0() //0 state is defualt state, want to reset card
    {
        currentState = 0;
        rectTransform.localScale = originalScale;
        rectTransform.localRotation = originalRotation;
        rectTransform.localPosition = originalPosition; //reset values
        //glowEffect.SetActive(false); //turn off glow and play arrow when card is reset
        //same for arrow
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition; //store original position when mouse enters card
            originalRotation = rectTransform.localRotation; //store original rotation when mouse enters card
            originalScale = rectTransform.localScale; //store original scale when mouse enters card

            currentState = 1; //hover state
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 2; //dragging state
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition); //store mouse position when dragging starts
            originalPanelLocalPosition = rectTransform.localPosition; //store card position when dragging starts
            latestPointerPosition = eventData.position; //added to remember the current pointer position without using Input.mousePosition
        }
    }

    public void OnPointerUp(PointerEventData eventData) //added release detection using UI event data instead of Input.GetMouseButton
    {
        if (currentState == 2 || currentState == 3) //added so releasing while dragging or playing resets the card
        {
            TransitionToState0(); //added reset when pointer/mouse button is released
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        latestPointerPosition = eventData.position; //added to keep the latest pointer position from the new Input System

        if (currentState == 2)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition))
            {
                //localPointerPosition /= canvas.scaleFactor;
                Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition; // all this  stuff is making sure the card is following the mouse and not jsut the mouse movement
                rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal; //move card with mouse

                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    currentState = 3; //play state
                    rectTransform.localPosition = playPosition; //move card to play position
                }
            }
        }
    }

    private void HandleHoverState()
    {
        //set glow effect and play arrow to active when hovered
        rectTransform.localScale = originalScale * selectScale; //scale up
    }

    private void HandleDragState()
    {
        //can add glow effect and play arrow here if wanted
        rectTransform.localRotation = Quaternion.identity; //reset rotation while dragging
    }

    private void HandlePlayState()
    {
        //can add glow effect and play arrow here if wanted
        rectTransform.localPosition = playPosition; //keep card at play position while in play state
        rectTransform.localRotation = Quaternion.identity; //reset rotation while playing

        if (latestPointerPosition.y < cardPlay.y) //changed from Input.mousePosition.y to stored pointer position from event data
        {
            currentState = 2; //if mouse goes back down, go back to dragging state
        }
    }
}