using UnityEngine;
using System.Collections;

public class GameManagerBehavior : MonoBehaviour
{
    private bool _isPlayerTurn = false;
    [SerializeField]
    private Card _playerCard, _enemyCard;
    private int _currentPlayerHealth, _currentEnemyHealth;
    private string _winner;

    void Start()
    {
        _currentPlayerHealth = _playerCard.health;
        print(_currentPlayerHealth);
        _currentEnemyHealth = _enemyCard.health;
        print(_currentEnemyHealth);
        StartCoroutine(AutoFight());
    }
    
    private void PlayTurn()
    {
        switch (_isPlayerTurn)
        {
            case true:
            {
                _currentEnemyHealth=_enemyCard.TakeDamage(_currentEnemyHealth, _playerCard.Attack());
                print(_currentEnemyHealth);
                break;
            }
            case false:
            {
                _currentPlayerHealth=_playerCard.TakeDamage(_currentPlayerHealth, _enemyCard.Attack());
                print(_currentPlayerHealth);
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
