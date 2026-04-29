\# CI/CD Pipeline Project - Todo API



\## Pipeline Description

My CI/CD flow is built using GitHub Actions. It is split into two stages:

1\. \*\*Continuous Integration (CI):\*\* Every time I push code to GitHub, the pipeline automatically installs .NET 8, restores dependencies, and runs 14 unit tests using xUnit. If any test fails, the pipeline stops and the deployment is blocked.

2\. \*\*Continuous Deployment (CD):\*\* If and only if the tests pass on the main branch, the pipeline sends a request to Render via a "Deploy Hook" to pull the new code and update the live site.



\## Deployment Strategy

I am using the \*\*Rolling Update\*\* strategy. 

This is the default for Render and is ideal for this project. When a new version is pushed, Render keeps the old version running while it builds the new one. It uses a health check at the `/health` endpoint. Once the new version is confirmed healthy, it switches the traffic over and shuts down the old version. This ensures the app has zero downtime during updates.



\## Rollback Guide

If a bug is discovered in production, here is how I would perform a rollback:

1\. \*\*Fast Rollback (Render):\*\* Go to the Render Dashboard, select the "Deploys" tab, find the last successful build, and click "Rollback to this revision." This immediately puts the old, working version back online.

2\. \*\*Permanent Fix (Git):\*\* I would use `git revert \[commit\_id]` locally to undo the bad changes, then push to GitHub. The pipeline will treat this as a new update, run the tests, and redeploy a stable version.

