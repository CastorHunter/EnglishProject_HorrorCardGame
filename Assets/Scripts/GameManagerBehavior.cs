using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;

public class GameManagerBehavior : MonoBehaviour
{
    private bool _isPlayerTurn = false;
    public Player player;
    public Entity enemyEntity;
    private int _currentPlayerHealth, _currentEnemyHealth, _currentPlayerStamina, _currentEnemyStamina;
    private string _winner;
    [SerializeField]
    private TextMeshProUGUI _playerHealthText, _enemyHealthText, _fightResultText;

    void Start()
    {
        //Set player health, speed and image, and reference itself
        _currentPlayerHealth = player.health;
        _currentPlayerStamina = player.speed;
        player.GetComponent<SpriteRenderer>().sprite = player.cardImage;
        player.gameManagerBehavior = this;
        //Set enemy health, speed and image, and reference itself
        _currentEnemyHealth = enemyEntity.health;
        _currentEnemyStamina = enemyEntity.speed;
        enemyEntity.GetComponent<SpriteRenderer>().sprite = enemyEntity.cardImage;
        enemyEntity.gameManagerBehavior = this;
        //Hide the result text pannel
        _fightResultText.enabled = false;
        //Start the autofight
        StartCoroutine(AutoFight());
    }
    
    private void PlayTurn()
    {
        StartCoroutine(AttackAnimation());
        switch (_isPlayerTurn)
        {
            case true:
            {
                _currentEnemyHealth=enemyEntity.TakeDamage(_currentEnemyHealth, player.Attack());
                ChangeHealthText(_enemyHealthText, _currentEnemyHealth);
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

    private void CheckWhoHasToPlay()
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

    private bool CheckIfSomeoneWon(bool someoneWon = false)
    {
        if (_currentEnemyHealth <= 0)
        {
            someoneWon = true;
            _winner = player.cardName;
        }

        if (_currentPlayerHealth <= 0)
        {
            someoneWon = true;
            _winner = enemyEntity.cardName;
        }
        return someoneWon;
    }

    private void ChangeHealthText(TextMeshProUGUI textToChange, int newHealth)
    {
        textToChange.text = ("Health : " + newHealth.ToString());
    }
    
    private IEnumerator AutoFight()
    {
        yield return new WaitForSeconds(1);
        CheckWhoHasToPlay();
        PlayTurn();
        if (CheckIfSomeoneWon())
        {
            _fightResultText.text = (_winner + " won");
            _fightResultText.enabled = true;
        }
        else
        {
            StartCoroutine(AutoFight());
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
