# Kubernetes Manifests

## Deploying all manifests

```sh
# Change directory into this directory
cd ./k8sManifests

# Create namespace
kubectl create namespace monitoring

# Create Gmail auth secret so that alerts can be posted via email (recommend using an "App Password" instead of real password)
kubectl create secret generic gmail-auth --from-literal=username='GMAIL_EMAIL' --from-literal=password='GMAIL_PASSWORD' --namespace=monitoring
# Create Slack API URL secret so that alerts can be posted to Slack
kubectl create secret generic slack-api-url --from-literal=url='SLACK_URL' --namespace=monitoring

# Deploy AirGradientDataServer app
kubectl apply -f ./airgradientdataserver.yml

# Deploy kube-state-metrics so that extra Kubernetes metrics can be read by Prometheus
# Latest config can be found here: https://github.com/kubernetes/kube-state-metrics/tree/master/examples/standard
kubectl apply -f ./kube-state-metrics

# Deploy Prometheus (for monitoring)
kubectl apply -f ./prometheus.yml

# Deploy Prometheus Alert Manager (for sending alerts when things go wrong)
kubectl apply -f ./alertmanager.yml
```
