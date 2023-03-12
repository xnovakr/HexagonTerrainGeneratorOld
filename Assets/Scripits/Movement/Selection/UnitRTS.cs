using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRTS : MonoBehaviour {

    private GameObject selectedGameObject;
    private Pathfinding movePosition;

    private void Awake() {
        selectedGameObject = transform.Find("SelectedCircle").gameObject;
        movePosition = GetComponent<Pathfinding>();
        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool visible) {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition) {
        if (!movePosition) return;
        movePosition.MoveTo(targetPosition);
    }

}
