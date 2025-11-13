using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManagerBehavior : MonoBehaviour
{
    //Temporary
    public TextMeshProUGUI scarecrowCrowsDefendText;
    
    private bool _isPlayerTurn = false, _canPlayACard = false, _autoFightOn = false, _gameIsOver = false;
    public Player player;
    public GameObject enemy;
    public Entity enemyEntity;
    private int _enemyLevel = 1, _playerLevel = 1;
    public int currentPlayerHealth, currentEnemyHealth, currentPlayerStamina, currentEnemyStamina, abyssalEndCountDown;
    private string _winner;
    [SerializeField]
    private TextMeshProUGUI _playerHealthText, _enemyHealthText, _fightResultText;
    private Coroutine _autoFightCoroutine;
    [SerializeField]
    private GameObject _travelMap, _playerposition, _enemyPosition;
    [SerializeField]
    private Button _card1, _card2, _card3;
    [SerializeField]
    private List<Card> _deck = new List<Card>();

    void Start()
    {
        //Hides the result text pannel
        _fightResultText.enabled = false;
        _playerHealthText.enabled = false;
        _enemyHealthText.enabled = false;
        player._playerStatesText.enabled = false;
        
        //Change the texts of the cards and disable them
        CanNotPlayACard();
        _card1.gameObject.SetActive(false);
        _card2.gameObject.SetActive(false);
        _card3.gameObject.SetActive(false);
        _card1.GetComponentInChildren<TextMeshProUGUI>().text = _deck[0].name;
        _card2.GetComponentInChildren<TextMeshProUGUI>().text = _deck[1].name;
        _card3.GetComponentInChildren<TextMeshProUGUI>().text = _deck[2].name;
        
        scarecrowCrowsDefendText.text = "";
    }

    public void StartFighting(int scriptedFightLevel) //Start the autofight
    {
        _travelMap.SetActive(false);
        _card1.gameObject.SetActive(true);
        _card2.gameObject.SetActive(true);
        _card3.gameObject.SetActive(true);
        switch (scriptedFightLevel)
        {
            case 1:
                enemyEntity = enemy.GetComponent<Scarecrow>();
                _enemyLevel = 1;
                break;
            case 2:
                enemyEntity = enemy.GetComponent<Mermaid>();
                _enemyLevel = 1;
                break;
            case 3:
                enemyEntity = enemy.GetComponent<Krasue>();
                _enemyLevel = 1;
                break;
        }

        player.gameManagerBehavior = this;
        _winner = null;
        player.ClearStates();
        
        player.gameObject.transform.position = _playerposition.transform.position;
        enemyEntity.gameObject.transform.position = _enemyPosition.transform.position;
        _playerHealthText.enabled = true;
        _enemyHealthText.enabled = true;
        player._playerStatesText.enabled = true;
        
        //Set player health, speed and image, and reference itself
        currentPlayerHealth = player.health*_playerLevel;
        currentPlayerStamina = player.speed*_playerLevel;
        player.GetComponent<SpriteRenderer>().sprite = player.cardImage;
        ChangeHealthText(_playerHealthText, currentPlayerHealth);
        
        //Set enemy health, speed and image, and reference itself
        currentEnemyHealth = enemyEntity.health*_enemyLevel;
        currentEnemyStamina = enemyEntity.speed*_enemyLevel;
        enemyEntity.GetComponent<SpriteRenderer>().sprite = enemyEntity.cardImage;
        enemyEntity.gameManagerBehavior = this;
        ChangeHealthText(_enemyHealthText, currentEnemyHealth);
        _gameIsOver = false;
        _autoFightCoroutine = StartCoroutine(AutoFight());
    }
    
    private void PlayTurn() //Play a turn after checking who has to play
    {
        switch (_isPlayerTurn)
        {
            case true:
            {
                if (_autoFightOn)
                {
                    StartCoroutine(AttackAnimation());
                    currentEnemyHealth=enemyEntity.TakeDamage(currentEnemyHealth, player.Attack());
                    ChangeHealthText(_enemyHealthText, currentEnemyHealth);
                    ApplyStates();
                }
                else
                {
                    CanPlayACard();
                }
                break;
            }
            case false:
            {
                StartCoroutine(AttackAnimation());
                currentPlayerHealth=player.TakeDamage(currentPlayerHealth, enemyEntity.Attack());
                ChangeHealthText(_playerHealthText, currentPlayerHealth);
                CheckIfSomeoneWon();
                if (_winner == null)
                {
                    _autoFightCoroutine = StartCoroutine(AutoFight());
                }
                break;
            }
        }
    }

    public void CardPlayed(int cardNumber)
    {
        if(_canPlayACard && _autoFightOn == false)
        {
            CanNotPlayACard();
            switch (cardNumber)
            {
                case 1:
                    _deck[0].gameManagerBehavior = this;
                    _deck[0].CardAction();
                    DrawCards(1);
                    break;
                case 2:
                    _deck[1].gameManagerBehavior = this;
                    _deck[1].CardAction();
                    DrawCards(2);
                    break;
                case 3:
                    _deck[2].gameManagerBehavior = this;
                    _deck[2].CardAction();
                    DrawCards(3);
                    break;
            }
            ApplyStates();
            ChangeHealthText(_enemyHealthText, currentEnemyHealth);
            ChangeHealthText(_playerHealthText, currentPlayerHealth);
            CheckIfSomeoneWon();
            if (_winner == null)
            {
                _autoFightCoroutine = StartCoroutine(AutoFight());
            }
        }
    }

    private void DrawCards(int cardUsed)
    {
        _deck.Add(_deck[cardUsed-1]);
        _deck.RemoveAt(cardUsed-1);
        _card1.GetComponentInChildren<TextMeshProUGUI>().text = _deck[0].name;
        _card2.GetComponentInChildren<TextMeshProUGUI>().text = _deck[1].name;
        _card3.GetComponentInChildren<TextMeshProUGUI>().text = _deck[2].name;
    }

    private void CanPlayACard()
    {
        _canPlayACard = true;
        _card1.interactable = true;
        _card2.interactable = true;
        _card3.interactable = true;
    }

    private void CanNotPlayACard()
    {
        _canPlayACard = false;
        _card1.interactable = false;
        _card2.interactable = false;
        _card3.interactable = false;
    }

    private void CheckWhoHasToPlay() //Check who has to play according to current stamina
    {
        if (currentPlayerStamina >= currentEnemyStamina)
        {
            _isPlayerTurn = true;
            currentEnemyStamina += enemyEntity.speed;
        }
        else
        {
            _isPlayerTurn = false;
            currentPlayerStamina += player.speed;
        }
    }

    private void CheckIfSomeoneWon() //End the game if someone won
    {
        if (currentEnemyHealth <= 0)
        {
            EndGame(player);
        }

        if (currentPlayerHealth <= 0)
        {
            EndGame(enemyEntity);
        }
    }

    private void ChangeHealthText(TextMeshProUGUI textToChange, int newHealth)
    {
        textToChange.text = ("Health : " + newHealth.ToString());
    }

    private void ApplyStates() //Apply states on the player
    {
        print("Apply States effects");
        if (player.GetStates().Contains(State.AbyssalEnd))
        {
            abyssalEndCountDown+=1;
            if (abyssalEndCountDown >= 5)
            {
                EndGame(enemyEntity);
                abyssalEndCountDown = 5;
            }
        }
        if (player.GetStates().Contains(State.Poisoned))
        {
            currentPlayerHealth -= 1;
            CheckIfSomeoneWon();
        }
    }

    private void EndGame(Entity entity) //Select a winner to end the game
    {
        _gameIsOver = true;
        _winner = entity.cardName;
        _fightResultText.text = (_winner + " won");
        _fightResultText.enabled = true;
        StartCoroutine(LaunchMap());
    }
    
    private IEnumerator LaunchMap() 
    {
        yield return new WaitForSeconds(3);
        _travelMap.SetActive(true);
        
        //Hides the result text pannel
        _fightResultText.enabled = false;
        _playerHealthText.enabled = false;
        _enemyHealthText.enabled = false;
        player._playerStatesText.enabled = false;
        
        //Change the texts of the cards and disable them
        CanNotPlayACard();
        _card1.gameObject.SetActive(false);
        _card2.gameObject.SetActive(false);
        _card3.gameObject.SetActive(false);
        _card1.GetComponentInChildren<TextMeshProUGUI>().text = _deck[0].name;
        _card2.GetComponentInChildren<TextMeshProUGUI>().text = _deck[1].name;
        _card3.GetComponentInChildren<TextMeshProUGUI>().text = _deck[2].name;
        
        
    }
    
    private IEnumerator AutoFight()
    {
        if (_gameIsOver == false)
        {
            yield return new WaitForSeconds(1);
            CheckWhoHasToPlay();
            PlayTurn();
        }
    }

    public IEnumerator AttackAnimation()
    {
        switch (_isPlayerTurn)
        {
            case true:
            {
                player.gameObject.transform.position = new Vector3(player.gameObject.transform.position.x+5, player.gameObject.transform.position.y, player.gameObject.transform.position.z);
                yield return new WaitForSeconds(0.2f);
                player.gameObject.transform.position = new Vector3(player.gameObject.transform.position.x-5, player.gameObject.transform.position.y, player.gameObject.transform.position.z);
                break;
            }
            case false:
            {
                enemyEntity.gameObject.transform.position = new Vector3(enemyEntity.gameObject.transform.position.x-5, enemyEntity.gameObject.transform.position.y, enemyEntity.gameObject.transform.position.z);
                yield return new WaitForSeconds(0.2f);
                enemyEntity.gameObject.transform.position = new Vector3(enemyEntity.gameObject.transform.position.x+5, enemyEntity.gameObject.transform.position.y, enemyEntity.gameObject.transform.position.z);
                break;
            }
        }
    }
}
