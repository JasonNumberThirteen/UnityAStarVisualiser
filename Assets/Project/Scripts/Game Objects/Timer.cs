using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
	public bool TimerWasStarted {get; private set;}
	public bool TimerWasFinished {get; private set;}
	
	public UnityEvent timerStartedEvent;
	public UnityEvent timerFinishedEvent;

	[SerializeField, Min(0f)] private float duration;

	private float currentTime;
	
	public void StartTimer()
	{
		SetAsFinished(false);
		timerStartedEvent?.Invoke();
	}

	public void InterruptTimerIfPossible(bool invokeReachedEndEvent = false)
	{
		if(TimerWasStarted)
		{
			FinishTimer(invokeReachedEndEvent);
		}
	}

	private void Update()
	{
		if(!TimerWasStarted)
		{
			return;
		}
		
		if(currentTime < duration)
		{
			currentTime = Mathf.Clamp(currentTime + Time.deltaTime, 0f, duration);
		}
		else if(!TimerWasFinished)
		{
			FinishTimer();
		}
	}

	private void FinishTimer(bool invokeReachedEndEvent = true)
	{
		SetAsFinished(true);

		if(invokeReachedEndEvent)
		{
			timerFinishedEvent?.Invoke();
		}
	}

	private void SetAsFinished(bool finished)
	{
		currentTime = finished ? duration : 0f;
		TimerWasStarted = !finished;
		TimerWasFinished = finished;
	}
}