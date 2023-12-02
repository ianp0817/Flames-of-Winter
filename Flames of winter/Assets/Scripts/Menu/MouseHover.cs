using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	Image image;

	void Start()
	{
		image = GetComponent<Image>();
		image.color = Color.white;
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
		image.color = Input.GetMouseButton(0) ? Color.grey : new(0.75f, 0.75f, 0.75f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		image.color = Color.white;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		image.color = Color.grey;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.hovered.Contains(gameObject))
			image.color = new(0.75f, 0.75f, 0.75f);
	}
}
