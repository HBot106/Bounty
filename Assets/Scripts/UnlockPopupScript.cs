using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPopupScript : MonoBehaviour
{
    public GameObject popupMenu;
    public void ExitPopupMenu()
    {
        popupMenu.SetActive( false );
        Cursor.lockState = CursorLockMode.Locked;
    }


}
