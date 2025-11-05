using UnityEngine;
using System.Collections;
using TMPro;

public class GameManagerBehavior : MonoBehaviour
{
    private bool _isPlayerTurn = false;
    public Card_Player _playerCard;
    public Card _enemyCard;
    private int _currentPlayerHealth, _currentEnemyHealth, _currentPlayerStamina, _currentEnemyStamina;
    private string _winner;
    [SerializeField]
    private TextMeshProUGUI _playerHealthText, _enemyHealthText, _fightResultText;

    void Start()
    {
        //Set player health, speed and image, and reference itself
        _currentPlayerHealth = _playerCard.health;
        _currentPlayerStamina = _playerCard.speed;
        _playerCard.GetComponent<SpriteRenderer>().sprite = _playerCard.cardImage;
        _playerCard.gameManagerBehavior = this;
        //Set enemy health, speed and image, and reference itself
        _currentEnemyHealth = _enemyCard.health;
        _currentEnemyStamina = _enemyCard.speed;
        _enemyCard.GetComponent<SpriteRenderer>().sprite = _enemyCard.cardImage;
        _enemyCard.gameManagerBehavior = this;
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
                _currentEnemyHealth=_enemyCard.TakeDamage(_currentEnemyHealth, _playerCard.Attack());
                ChangeHealthText(_enemyHealthText, _currentEnemyHealth);
                break;
            }
            case false:
            {
                _currentPlayerHealth=_playerCard.TakeDamage(_currentPlayerHealth, _enemyCard.Attack());
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
            _currentEnemyStamina += _enemyCard.speed;
        }
        else
        {
            _isPlayerTurn = false;
            _currentPlayerStamina += _playerCard.speed;
        }
    }

    private bool CheckIfSomeoneWon(bool someoneWon = false)
    {
        if (_currentEnemyHealth <= 0)
        {
            someoneWon = true;
            _winner = _playerCard.cardName;
        }

        if (_currentPlayerHealth <= 0)
        {
            someoneWon = true;
            _winner = _enemyCard.cardName;
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
                _playerCard.gameObject.transform.position = new Vector3(_playerCard.gameObject.transform.position.x+5, _playerCard.gameObject.transform.position.y, _playerCard.gameObject.transform.position.z);
                yield return new WaitForSeconds(0.2f);
                _playerCard.gameObject.transform.position = new Vector3(_playerCard.gameObject.transform.position.x-5, _playerCard.gameObject.transform.position.y, _playerCard.gameObject.transform.position.z);
                break;
            }
            case false:
            {
                _enemyCard.gameObject.transform.position = new Vector3(_enemyCard.gameObject.transform.position.x-5, _enemyCard.gameObject.transform.position.y, _enemyCard.gameObject.transform.position.z);
                yield return new WaitForSeconds(0.2f);
                _enemyCard.gameObject.transform.position = new Vector3(_enemyCard.gameObject.transform.position.x+5, _enemyCard.gameObject.transform.position.y, _enemyCard.gameObject.transform.position.z);
                break;
            }
        }
    }
}
