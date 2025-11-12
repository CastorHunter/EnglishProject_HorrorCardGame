using UnityEngine;

public class Card : MonoBehaviour
{
    public string cardName;
    public GameManagerBehavior gameManagerBehavior;
    
    public virtual void CardAction()
    {
        print("CardAction");
    }
}
