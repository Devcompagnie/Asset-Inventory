using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    GameObject takeObject; // Objet que l'on prend
    ObjectToTake objectToTake; // script pour prendre un objet
    public GameObject Player; // Le joueur dans la scène
    public Image SelectionMouseImage; // Image permettant visuellement de savoir quel est la case de l'inventaire qui est sélectionée
    int SelectionMouse; // permet de connaitre quel case de l'inventaire est sélectionée

    public GameObject PannelInventoryFull; // Pannel permettant d'avoir l'information visuellement que l'inventaire est plein

    [Header("Représentation img")]
    public Sprite[] Representations; // avoir toutes les représentations des images

    [Header("Actual Inventory")]
    public Image[] InventoryActual;// Placer les cases d'inventaires en jeu et non en mode inventaire

    int SelectedPlaceOfInventory; // Quand la souris est sur un emplacement de l'inventaire

    public Sprite ImageToStock;

    bool IsFull; // Savoir si l'inventaire est plein ou non
    int LengthInventory; // Le max de l'inventaire

    int PasFull;// pour avoir le nombre de cases incomplètes
    int Full; // pour avoir le nombre de cases complètes

    public float alarm = 3;// temps que dure l'alarme
    bool CallAlarm; // appel de l'alarme

    public GameObject[] ObjectImagesReferences;//Images des objets 3D qu'il y a dans l'inventaire
    public GameObject[] ObjectModel3dReference;//modèle 3D des objets que l'on peut prendre
    public int ActualObject;//Objet qu'on prend sur le moment

    Vector3 RespawnObject; // Où l'objet va respawn

    public KeyCode[] KeyUseForInventory; // Touches pour switch les cases de l'inventaire
    // Start is called before the first frame update
    void Start()
    {
        PannelInventoryFull.active = false;
        LengthInventory = InventoryActual.Length;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown(KeyUseForInventory[0])) // si jamais on appuie sur le bouton droit alors
        {
            SelectionMouse++; // la sélection de la souris vaut la sélection de la souris + 1
            Debug.Log(SelectionMouse); // affichage dans la console
        }
        if (Input.GetKeyDown(KeyUseForInventory[1]) || Input.GetAxis("Mouse ScrollWheel") > 0)// si jamais on appuie sur le bouton gauche alors
        {
            SelectionMouse--; // la sélection de la souris vaut la sélection de la souris - 1
            Debug.Log(SelectionMouse);// affichage dans la console
        }

        if (SelectionMouse == -1)//Si la sélection de la souris est inférieur à 0 alors on vient à la fin de l'inventaire
        {
            SelectionMouse = InventoryActual.Length - 1;
        }
        if (SelectionMouse == LengthInventory)//Si la sélection de la souris est supérieur à la fin de l'inventaire alors on vient au début de l'inventaire
        {
            SelectionMouse = 0;
        }

        if (Input.GetKeyDown("r"))//Si on appuie sur r alors on appel la fonction "Lacher"
        {
            Lacher();
        }

        SelectionMouseImage.transform.position = InventoryActual[SelectionMouse].transform.position; // L'image permettant visuellemnt de savoir quelle case de l'inventaire est sélectionnée se déplace en fonction de la variable "SelectionMouse"

        if (CallAlarm == true) // l'alarme est appelée alors la valeur de l'alarme est soustrait par 1 à chaque seconde
        {
            alarm -= Time.deltaTime;
        }
        if (alarm <= 0) // si l'alarme est inférieur ou égale à 0 alors l'appelle de l'alarme est désactivée est tout les pannels sont désactiver
        {
            PannelInventoryFull.active = false;
            CallAlarm = false;
            alarm = 3;
        }
    }

    // Pour prendre un objet
    public void TakeObject (int TypeOfObject, int u_object, ObjectToTake scriptObject) // Fonction pour prendre un objet
    {
        objectToTake = scriptObject; // Le script "ObjectToTake" est le script de l'objet que l'on prend
        ActualObject = u_object; // l'objet actuel est égal à un objet référencé dans tout les objets 3D pour l'inventaire
        ImageToStock = Representations[TypeOfObject];// l'image de l'objet est égal à une image référencée dans toutes les images pour l'inventaire
        Debug.Log("Vérification de l'inventaire");
        VerifFull();// appel de la fonction "VerifFull"
        FinishVerif();// appel de la fonction "FinishVerif"
        if (IsFull == false) // Si jamais l'inventaire n'est pas complet alors on le rajoute
        {
            objectToTake.ToDestroy = true;
            Debug.Log("Insertion du sprite");
            GoInventory();
            objectToTake = null;
        }
    }

    //Placement dde l'image
    void GoInventory ()
    {
        for (int i = 0; i < InventoryActual.Length; i ++)
        {
            if (InventoryActual[i].sprite == null)
            {
                ObjectModel3dReference[i] = ObjectImagesReferences[ActualObject];
                InventoryActual[i].sprite = ImageToStock;
                Debug.Log("Sprite inséré");
                i = LengthInventory + 1;
                ImageToStock = null;
            }
        }
    }

    // Vérifier si l'inventaire est plein
    void VerifFull ()
    {
        

        for (int i = 0; i <InventoryActual.Length; i ++)
        {
            if (InventoryActual[i].sprite != null)
            {
                Full++;
            }
            else if (InventoryActual[i].sprite == null)
            {
                PasFull++;
            }
        }
        
    }
    // l'inventaire est plein
    void InventorFull ()
    {
        CallAlarm = true;
        PannelInventoryFull.active = true;

       
    }
    // Donner les résultats de l'opération de la vérification
    void FinishVerif ()
    {
        if (PasFull > 0)
        {
            IsFull = false;
            Debug.Log("Inventaire pas plein");
            PasFull = 0;
            Full = 0;
        }
        else if (Full == InventoryActual.Length)
        {
            IsFull = true;
            Debug.Log("Inventaire plein");
            PasFull = 0;
            Full = 0;
            InventorFull();
        }
    }
    // l'acher l'objet sélectioné
    void Lacher ()
    {
        Instantiate(ObjectModel3dReference[SelectionMouse], Player.transform.position, gameObject.transform.rotation);
        ObjectModel3dReference[SelectionMouse] = null;
        InventoryActual[SelectionMouse].sprite = null;
    }
}
