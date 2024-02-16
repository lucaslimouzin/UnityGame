using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connecte au serveur Photon en utilisant les paramètres par défaut.
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connecté au serveur Photon.");
        PhotonNetwork.JoinRandomRoom(); // Tente de rejoindre une salle aléatoire.
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Échec de la connexion à une salle aléatoire. Création d'une nouvelle salle.");
        PhotonNetwork.CreateRoom(null, new RoomOptions()); // Crée une nouvelle salle si la connexion à une salle aléatoire échoue.
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connecté à une salle.");
        // Ici, vous pouvez initialiser le spawn de votre joueur ou d'autres éléments du jeu.
    }
}
