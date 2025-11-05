using UnityEngine;
using System.Collections;
using TMPro;

public class GameManagerBehavior : MonoBehaviour
{
    private bool _isPlayerTurn = false;
    [SerializeField]
    private Card _playerCard, _enemyCard;
    private int _currentPlayerHealth, _currentEnemyHealth, _currentPlayerStamina, _currentEnemyStamina;
    private string _winner;
    [SerializeField]
    private TextMeshProUGUI _playerHealthText, _enemyHealthText;

    void Start()
    {
        _currentPlayerHealth = _playerCard.health;
        print(_currentPlayerHealth);
        _currentEnemyHealth = _enemyCard.health;
        print(_currentEnemyHealth);
        StartCoroutine(AutoFight());
        _playerCard.GetComponent<SpriteRenderer>().sprite = _playerCard.cardImage;
        _enemyCard.GetComponent<SpriteRenderer>().sprite = _enemyCard.cardImage;
    }
    
    private void PlayTurn()
    {
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

    private bool CheckIfSomeoneWon(bool someoneWon = false)
    {
        if (_currentEnemyHealth <= 0)
        {
            someoneWon = true;
            _winner = _playerCard.name;
        }

        if (_currentPlayerHealth <= 0)
        {
            _winner = _enemyCard.name;
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
        _isPlayerTurn = !_isPlayerTurn;
        PlayTurn();
        if (CheckIfSomeoneWon())
        {
            print(_winner + " won");
        }
        else
        {
            StartCoroutine(AutoFight());
        }
    }
}
