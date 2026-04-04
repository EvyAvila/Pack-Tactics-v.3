using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class AbstractMenuBase : MonoBehaviour
{
    public VisualElement root;
    public UIDocument uiDocument;

    public abstract void SetProperties();
    public abstract void UnSetProperties();

    public virtual void OnEnable()
    {
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
            //Debug.LogError("PauseMenu: UIDocument is not assigned!");
            //return;
        }

        StartCoroutine(SetupUI());
    }

    public virtual void OnDisable()
    {
        UnSetProperties();
    }

    public virtual IEnumerator SetupUI()
    {
        yield return null;

        root = uiDocument.rootVisualElement;
        SetProperties();
    }
}
