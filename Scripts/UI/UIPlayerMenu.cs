using Malimbe.XmlDocumentationAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIPlayerMenu : UIDelegate
{
    #region Player Menu Settings
    [field: Header("Menu Settings"), DocumentedByXml]
    public List<GameObject> taskPanel;
    public List<GameObject> taskPanelButtons;
    public GameObject modulesPanel = null;
    public GameObject tutorialPrompt= null;
    #endregion

    public delegate void OnReturnClick(GameObject taskPanel);


    public override void Start()
    {
        base.Start();
        Debug.Assert(modulesPanel != null);
        Debug.Assert(tutorialPrompt != null);
        Debug.Log("UIPlayerMenu Start");
        // assigns delegate(s) to button(s)
        for (int i = 0; i < taskPanel.Count; i++)
        {
            AddReturnButtonDelegate(taskPanel[i], taskPanelButtons[i], returnToModuleList);
        }
        

        // Update menu state here using GameState instance object
    }

    public void AddReturnButtonDelegate(GameObject panel, GameObject returnButton, OnReturnClick handler)
    {
        Button button = returnButton.GetComponent<Button>();
        button.onClick.AddListener(delegate { handler(panel); });
    }

    public override void OnCloseAndContinue()
    {
        UIDriver.instance.HidePlayerMenu();
        if (GameState.inTutorial)
        {
            tutorialPrompt.SetActive(true);
            GameState.inTutorial = false;
        }
    }

    public void returnToModuleList(GameObject taskPanel)
    {
        taskPanel.SetActive(false);
        modulesPanel.SetActive(true);
    }
}
