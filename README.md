# homeside-code-kata


## Overview

Goal of this exercise is to build a web portal that allows all departments to upload (csv documents), download(csv documents) and submit tasks that automate various internal manual efforts. This will become the backbone of all department automations and must be able to quantify the work automated.
 
## Requirements

1. MVC .net 4.6, c#, entity framework, sql server 2014 express, git (source control)
2. Build a portal (no authentication) that uses our (gohomeside.com) theme.
3. User interface should have buttons / options for “Pay MIP”, ”Post Transactions”, “Create warehouse line file”
4. Action for each option will allow the user to upload a csv file and submit the task
5. Each submission should create a new task in the database with the following (create a normalized data scheme)
	a. Task type (think about using this in #7 to create a structure that will us to plug in different actions or automations for each type)
	b. Task name
	c. Task description
	d. Start datetime
	e. End datetime
	f. User who submitted the task
	g. Status (queued, processing, finished, error)
	h. File path / url to file uploaded
6. create a status page that utilizes jquery datatables to show each task in queue and the status. Order by date created. You should have 1 datatable. It must convey to the user which tasks are queued, processing, failed and successful.
7. Lastly, create a c# windows service that queries the database you created, for all tasks in queued status.
8. For now, don’t worry about the actual actions / code to implement each. Just set the end datetime and go onto the next queued item ( later, if hired, we will work on the automation / action code)
	a. Think about a scalable way to implement the actions so that over the next few years will have reusable actions that can be plugged into / reused. You don’t have to necessarily create all of it now but be able to speak to your plan on how we can implement this idea.
9. Lastly the windows service must process tasks in parallel (concurrently using multi-threading) and update the database immediately when a task is pulled off the queue (setting status to processing etc).

 
## The Review
-          Must be able to demo this application to Dan and I next week.
-          Must be able to review code and speak to why decisions were made.
-          Must be able to suggest other designs or technologies that would work better
-          Suggestion on how to log into various websites to automate clicking  and submitting. Frameworks?
-          Give us a brief overview of how you would track analytics given one action / task saves x amount of time.
-          All must work on your laptop. Let’s not worry about installing / deploying the solution.