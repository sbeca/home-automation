# Kubernetes Manifests

## Deploying all manifests

```sh
# Change directory into this directory
cd ./k8sManifests

# Create namespace
kubectl create namespace monitoring

# Deploy AirGradientDataServer app
kubectl apply -f ./airgradientdataserver.yml

# Deploy kube-state-metrics so that extra Kubernetes metrics can be read by Prometheus
# Latest config can be found here: https://github.com/kubernetes/kube-state-metrics/tree/master/examples/standard
kubectl apply -f ./kube-state-metrics

# Deploy Prometheus (for monitoring)
kubectl apply -f ./prometheus.yml
```
