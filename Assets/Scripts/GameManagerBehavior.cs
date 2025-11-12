using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour
{
    private bool _isPlayerTurn = false, _canPlayACard = false, _autoFightOn = false;
    public Player player;
    public GameObject enemy;
    public Entity enemyEntity;
    private int _currentPlayerHealth, _currentEnemyHealth, _currentPlayerStamina, _currentEnemyStamina, _abyssalEndCountDown, _enemyLevel = 1, _playerLevel = 1;
    private string _winner;
    [SerializeField]
    private TextMeshProUGUI _playerHealthText, _enemyHealthText, _fightResultText;
    private Coroutine _AutoFightCoroutine;
    [SerializeField]
    private GameObject _travelMap, _playerposition, _enemyPosition;
    [SerializeField]
    private Button _card1, _card2, _card3;

    void Start()
    {
        //Hides the result text pannel
        _fightResultText.enabled = false;
        _playerHealthText.enabled = false;
        _enemyHealthText.enabled = false;
        player._playerStatesText.enabled = false;
        
        //Hides the cards and disable them
        CanNotPlayACard();
        _card1.gameObject.SetActive(false);
        _card2.gameObject.SetActive(false);
        _card3.gameObject.SetActive(false);
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
        player.gameObject.transform.position = _playerposition.transform.position;
        enemyEntity.gameObject.transform.position = _enemyPosition.transform.position;
        _playerHealthText.enabled = true;
        _enemyHealthText.enabled = true;
        player._playerStatesText.enabled = true;
        
        //Set player health, speed and image, and reference itself
        _currentPlayerHealth = player.health*_playerLevel;
        _currentPlayerStamina = player.speed*_playerLevel;
        player.GetComponent<SpriteRenderer>().sprite = player.cardImage;
        player.gameManagerBehavior = this;
        ChangeHealthText(_playerHealthText, _currentPlayerHealth);
        
        //Set enemy health, speed and image, and reference itself
        _currentEnemyHealth = enemyEntity.health*_enemyLevel;
        _currentEnemyStamina = enemyEntity.speed*_enemyLevel;
        enemyEntity.GetComponent<SpriteRenderer>().sprite = enemyEntity.cardImage;
        enemyEntity.gameManagerBehavior = this;
        ChangeHealthText(_enemyHealthText, _currentEnemyHealth);
        _AutoFightCoroutine = StartCoroutine(AutoFight());
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
                    _currentEnemyHealth=enemyEntity.TakeDamage(_currentEnemyHealth, player.Attack());
                    ChangeHealthText(_enemyHealthText, _currentEnemyHealth);
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
                _currentPlayerHealth=player.TakeDamage(_currentPlayerHealth, enemyEntity.Attack());
                ChangeHealthText(_playerHealthText, _currentPlayerHealth);
                CheckIfSomeoneWon();
                if (_winner == null)
                {
                    _AutoFightCoroutine = StartCoroutine(AutoFight());
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
                    StartCoroutine(AttackAnimation());
                    _currentEnemyHealth=enemyEntity.TakeDamage(_currentEnemyHealth, player.Attack());
                    ChangeHealthText(_enemyHealthText, _currentEnemyHealth);
                    break;
                case 2:
                    StartCoroutine(AttackAnimation());
                    _currentEnemyHealth=enemyEntity.TakeDamage(_currentEnemyHealth, player.Attack());
                    ChangeHealthText(_enemyHealthText, _currentEnemyHealth);
                    break;
                case 3:
                    StartCoroutine(AttackAnimation());
                    _currentEnemyHealth=enemyEntity.TakeDamage(_currentEnemyHealth, player.Attack());
                    ChangeHealthText(_enemyHealthText, _currentEnemyHealth);
                    break;
            }
            ApplyStates();
            CheckIfSomeoneWon();
            if (_winner == null)
            {
                _AutoFightCoroutine = StartCoroutine(AutoFight());
            }
        }
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
        if (_currentPlayerStamina >= _currentEnemyStamina)
        {
            _isPlayerTurn = true;
            _currentEnemyStamina += enemyEntity.speed;
        }
        else
        {
            _isPlayerTurn = false;
            _currentPlayerStamina += player.speed;
        }
    }

    private void CheckIfSomeoneWon() //End the game if someone won
    {
        if (_currentEnemyHealth <= 0)
        {
            EndGame(player);
        }

        if (_currentPlayerHealth <= 0)
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
            _abyssalEndCountDown+=1;
            if (_abyssalEndCountDown >= 5)
            {
                EndGame(enemyEntity);
            }
        }
    }

    private void EndGame(Entity entity) //Select a winner to end the game
    {
        _winner = entity.cardName;
        _fightResultText.text = (_winner + " won");
        _fightResultText.enabled = true;
    }
    
    private IEnumerator AutoFight()
    {
        yield return new WaitForSeconds(1);
        CheckWhoHasToPlay();
        PlayTurn();
    }

    private IEnumerator AttackAnimation()
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
