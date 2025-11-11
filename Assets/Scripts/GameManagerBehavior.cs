using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;

public class GameManagerBehavior : MonoBehaviour
{
    private bool _isPlayerTurn = false;
    public Player player;
    public GameObject enemy;
    public Entity enemyEntity;
    private int _currentPlayerHealth, _currentEnemyHealth, _currentPlayerStamina, _currentEnemyStamina, _abyssalEndCountDown;
    private string _winner;
    [SerializeField]
    private TextMeshProUGUI _playerHealthText, _enemyHealthText, _fightResultText;
    private Coroutine _AutoFightCoroutine;
    [SerializeField]
    private GameObject _travelMap, _playerposition, _enemyPosition;

    void Start()
    {
        //Hide the result text pannel
        _fightResultText.enabled = false;
        _playerHealthText.enabled = false;
        _enemyHealthText.enabled = false;
        player._playerStatesText.enabled = false;
    }

    public void StartFighting(int scriptedFightLevel) //Start the autofight
    {
        _travelMap.SetActive(false);
        switch (scriptedFightLevel)
        {
            case 1:
                enemyEntity = enemy.GetComponent<Scarecrow>();
                break;
            case 2:
                enemyEntity = enemy.GetComponent<Mermaid>();
                break;
            case 3:
                enemyEntity = enemy.GetComponent<Krasue>();
                break;
        }
        player.gameObject.transform.position = _playerposition.transform.position;
        enemyEntity.gameObject.transform.position = _enemyPosition.transform.position;
        _playerHealthText.enabled = true;
        _enemyHealthText.enabled = true;
        player._playerStatesText.enabled = true;
        
        //Set player health, speed and image, and reference itself
        _currentPlayerHealth = player.health;
        _currentPlayerStamina = player.speed;
        player.GetComponent<SpriteRenderer>().sprite = player.cardImage;
        player.gameManagerBehavior = this;
        ChangeHealthText(_playerHealthText, _currentPlayerHealth);
        
        //Set enemy health, speed and image, and reference itself
        _currentEnemyHealth = enemyEntity.health;
        _currentEnemyStamina = enemyEntity.speed;
        enemyEntity.GetComponent<SpriteRenderer>().sprite = enemyEntity.cardImage;
        enemyEntity.gameManagerBehavior = this;
        ChangeHealthText(_enemyHealthText, _currentEnemyHealth);
        _AutoFightCoroutine = StartCoroutine(AutoFight());
    }
    
    private void PlayTurn() //Play a turn after checking who has to play
    {
        StartCoroutine(AttackAnimation());
        switch (_isPlayerTurn)
        {
            case true:
            {
                _currentEnemyHealth=enemyEntity.TakeDamage(_currentEnemyHealth, player.Attack());
                ChangeHealthText(_enemyHealthText, _currentEnemyHealth);
                ApplyStates();
                break;
            }
            case false:
            {
                _currentPlayerHealth=player.TakeDamage(_currentPlayerHealth, enemyEntity.Attack());
                ChangeHealthText(_playerHealthText, _currentPlayerHealth);
                break;
            }
        }
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
        CheckIfSomeoneWon();
        if (_winner == null)
        {
            _AutoFightCoroutine = StartCoroutine(AutoFight());
        }
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
