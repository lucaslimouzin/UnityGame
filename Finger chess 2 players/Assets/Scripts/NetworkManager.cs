using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro; // Assurez-vous d'inclure cet espace de noms pour accéder à TextMeshPro
using System.Collections;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Assurez-vous que ce GameObject possède un PhotonView dans l'éditeur Unity.
    private PhotonView photonView;
    public TextMeshProUGUI statusText; // Référence à l'objet TextMeshProUGUI
    public string gameplaySceneName = "Online_Gameplay";

    void Awake()
    {
        // Important pour synchroniser le chargement de la scène pour tous les joueurs
        PhotonNetwork.AutomaticallySyncScene = true;
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        UpdateStatusText("Connexion au serveur Photon...");
    }

    public override void OnConnectedToMaster()
    {
        UpdateStatusText("Recherche d'un adversaire à votre hauteur...");
        PhotonNetwork.JoinRandomRoom(); // Tente de rejoindre une salle aléatoire.
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //Debug.Log("OnJoinRandomFailed appelé, création d'une nouvelle salle...");
        // Définir aléatoirement la couleur pour le créateur de la salle
        bool isBlack = Random.value > 0.5f;
        string playerColor = isBlack ? "noir" : "blanc";
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 2, // Définir le nombre maximum de joueurs
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "playerColor", playerColor } },
            CustomRoomPropertiesForLobby = new string[] { "playerColor" }
        };
        PhotonNetwork.CreateRoom(null, options);
    }

    public override void OnCreatedRoom()
    {
        bool isBlack = Random.value > 0.5f;
        string playerColor = isBlack ? "noir" : "blanc";
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "playerColor", playerColor } });
    }



    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Player masterClient = PhotonNetwork.MasterClient;
            string masterColor = masterClient.CustomProperties["playerColor"] as string;
            string playerColor = (masterColor == "noir") ? "blanc" : "noir";
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "playerColor", playerColor } });
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("playerColor"))
        {
            // Le MasterClient est toujours le joueur 1
            int playerNumber = targetPlayer.IsMasterClient ? 1 : 2;

            // Un joueur a reçu une couleur, faites quelque chose avec cette information
            Debug.Log("Joueur " + playerNumber + " a la couleur " + changedProps["playerColor"]);
        }
    }





    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            // Le second joueur récupère la couleur opposée
            Player firstPlayer = PhotonNetwork.MasterClient;
            string firstPlayerColor = (string)firstPlayer.CustomProperties["playerColor"];
            string secondPlayerColor = firstPlayerColor == "noir" ? "blanc" : "noir";

            // Attribuer la couleur opposée au second joueur
            PhotonNetwork.LocalPlayer.CustomProperties["playerColor"] = secondPlayerColor;
            PhotonNetwork.LocalPlayer.SetCustomProperties(PhotonNetwork.LocalPlayer.CustomProperties);

            // Informer le MasterClient de la couleur attribuée au second joueur
            photonView.RPC("NotifyColorAssignment", firstPlayer, secondPlayerColor);
        }

        // Démarrez le compte à rebours si la salle est pleine
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(StartGameCountdown());
            }
        }
    }


    IEnumerator StartGameCountdown()
    {
        int countdown = 3;
        while (countdown > 0)
        {
            photonView.RPC("UpdateStatusTextRPC", RpcTarget.All, "Le match commence dans " + countdown + "...");
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        UpdateStatusTextRPC("Chargement...");

        // Le MasterClient charge la scène pour tous les joueurs dans la salle
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(gameplaySceneName);
        }
    }

    [PunRPC]
    void UpdateStatusTextRPC(string message)
    {
        //Debug.Log("RPC appelé avec message : " + message);
        UpdateStatusText(message);
    }

    void UpdateStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message; // Met à jour le texte de l'objet TextMeshProUGUI
        }
    }
}
