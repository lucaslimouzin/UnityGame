using System.Collections;
using UnityEngine;

public class CheckZoneDicePlayer : MonoBehaviour
{
    private int compteurDesPlayer = 0;
    void Start (){
        MainGameManager.Instance.checkFaitDesPlayer = true;
        MainGameManager.Instance.checkFaitDesMj = true;
        compteurDesPlayer = 0;
    }

    
    private void OnTriggerStay(Collider col)
    {
       if (MainGameManager.Instance.checkFaitDesPlayer == false){
            compteurDesPlayer += 1;
             
            switch (col.tag)
                {
                    case "SIDE1":
                        if (compteurDesPlayer >150){
                            
                            MainGameManager.Instance.scoreDesPlayer = 6;
                            compteurDesPlayer = 0;
                            MainGameManager.Instance.checkFaitDesPlayer = true;
                            //debug.Log("Player = " + MainGameManager.Instance.scoreDesPlayer);
                        }                   
                        break;
                    case "SIDE2":
                        if (compteurDesPlayer >150){
                             
                            MainGameManager.Instance.scoreDesPlayer = 5;
                            compteurDesPlayer = 0;
                            MainGameManager.Instance.checkFaitDesPlayer = true;
                            //debug.Log("Player = " + MainGameManager.Instance.scoreDesPlayer);
                        }        
                        break;
                    case "SIDE3":
                        if (compteurDesPlayer >150){
                             
                            MainGameManager.Instance.scoreDesPlayer = 4;
                            compteurDesPlayer = 0;
                            MainGameManager.Instance.checkFaitDesPlayer = true;
                            //debug.Log("Player = " + MainGameManager.Instance.scoreDesPlayer);
                        }        
                        break;
                    case "SIDE4":
                        if (compteurDesPlayer >150){
                             
                            MainGameManager.Instance.scoreDesPlayer = 3;
                            compteurDesPlayer = 0;
                            MainGameManager.Instance.checkFaitDesPlayer = true;
                            //debug.Log("Player = " + MainGameManager.Instance.scoreDesPlayer);
                        }        
                        break;
                    case "SIDE5":
                        if (compteurDesPlayer >150){
                             
                            MainGameManager.Instance.scoreDesPlayer = 2;
                            compteurDesPlayer = 0;
                            MainGameManager.Instance.checkFaitDesPlayer = true;
                            //debug.Log("Player = " + MainGameManager.Instance.scoreDesPlayer);
                        }        
                        break;
                    case "SIDE6":
                        if (compteurDesPlayer >150){
                             
                            MainGameManager.Instance.scoreDesPlayer = 1;
                            compteurDesPlayer = 0;
                            MainGameManager.Instance.checkFaitDesPlayer = true;
                            //debug.Log("Player = " + MainGameManager.Instance.scoreDesPlayer);
                        }        
                        break;
                    
                }
       }     
    }
}
