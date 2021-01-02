using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TextSystem : MonoBehaviour
{
    public InputField journalField;
    public InputField nameField;
    public Text buttonText;
    private Flower focusFlower;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Flower>() != null)
        {
            setFlower(collision.gameObject);
        }
    }
    private void Start()
    {
        focusFlower = null;
        noSelectedFlower();
    }

    public void noSelectedFlower()
    {
        journalField.text = "Select a Flower";
        nameField.text = ":-)";
        journalField.interactable = false;
        nameField.interactable = false;
    }
    public void saveText()
    {
        if (focusFlower != null)
        {
            focusFlower.text = journalField.text;
            focusFlower.userName = nameField.text;
         }
    }
 
    public void toggleButtonText()
    {
        if (nameField.interactable == true)
        {
            buttonText.text = "Save";
        }
        else
            buttonText.text = "Edit";
    }

    public void setFlower(GameObject selectedFlower)
    {
        Flower newF = selectedFlower.GetComponent<Flower>();
        if (newF != null)
        {
            focusFlower = newF;
            journalField.text = newF.getText();
            nameField.text = newF.getUserName();
        }
        
    }

    public void toggleInputFieldInteractable()
    {
        nameField.interactable = !nameField.interactable;
        journalField.interactable = !journalField.interactable;
        if (nameField.interactable == true)
        {
            buttonText.text = "Save";
        }
        else
            buttonText.text = "Edit";
    }

    public void sceneChange()
    {
        SceneManager.LoadScene("DrawScene");
    }

    public void QuitApp()
    {
    #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
    #else
             Application.Quit();
    #endif
    }
}
