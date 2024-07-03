using System.Collections;
using UnityEngine;

public class CheckZoneDiceMj : MonoBehaviour
{
    private int compteurDesMj = 0;

    void Start (){
        MainGameManager.Instance.checkFaitDesPlayer = true;
        MainGameManager.Instance.checkFaitDesMj = true;
        compteurDesMj = 0;

    }
    private void OnTriggerStay(Collider other)
    {
       if (MainGameManager.Instance.checkFaitDesMj == false){
            compteurDesMj += 1;
             
            switch (other.tag)
                {
                    case "SIDEMJ1":
                        if (compteurDesMj >150){
                            MainGameManager.Instance.scoreDesMj = 6;
                            
                            compteurDesMj = 0;
                            MainGameManager.Instance.checkFaitDesMj = true;
                            //debug.Log("Mj = " + MainGameManager.Instance.scoreDesMj);
                        }        
                        break;
                    case "SIDEMJ2":
                        if (compteurDesMj >150){
                            MainGameManager.Instance.scoreDesMj = 5;
                            
                            compteurDesMj = 0;
                            MainGameManager.Instance.checkFaitDesMj = true;
                            //debug.Log("Mj = " + MainGameManager.Instance.scoreDesMj);
                        }
                        break;
                    case "SIDEMJ3":
                        if (compteurDesMj >150){
                            MainGameManager.Instance.scoreDesMj = 4;
                            
                            compteurDesMj = 0;
                            MainGameManager.Instance.checkFaitDesMj = true;
                            //debug.Log("Mj = " + MainGameManager.Instance.scoreDesMj);
                        }
                        break;
                    case "SIDEMJ4":
                        if (compteurDesMj >150){
                            MainGameManager.Instance.scoreDesMj = 3;
                            
                            compteurDesMj = 0;
                            MainGameManager.Instance.checkFaitDesMj = true;
                            //debug.Log("Mj = " + MainGameManager.Instance.scoreDesMj);
                        }
                        break;
                    case "SIDEMJ5":
                        if (compteurDesMj >150){
                            MainGameManager.Instance.scoreDesMj = 2;
                            
                            compteurDesMj = 0;
                            MainGameManager.Instance.checkFaitDesMj = true;
                            //debug.Log("Mj = " + MainGameManager.Instance.scoreDesMj);
                        }
                        break;
                    case "SIDEMJ6":
                        if (compteurDesMj >150){
                            MainGameManager.Instance.scoreDesMj = 1;
                            
                            compteurDesMj = 0;
                            MainGameManager.Instance.checkFaitDesMj = true;
                            //debug.Log("Mj = " + MainGameManager.Instance.scoreDesMj);
                        }
                        break;
                }
       }     
    }
}
