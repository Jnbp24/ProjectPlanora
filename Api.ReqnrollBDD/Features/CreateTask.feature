Feature: CreateTask

Scenario: User Creates a Task
	Given User is logged in
	And User has filled in Task info
	When User clicks Create Task button
	Then Task is created