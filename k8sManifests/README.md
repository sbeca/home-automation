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

# Deploy node-exporter so that metrics for each node can be read by Prometheus
kubectl apply -f ./node-exporter.yml

# Deploy kube-state-metrics so that extra Kubernetes metrics can be read by Prometheus
# Latest config can be found here: https://github.com/kubernetes/kube-state-metrics/tree/master/examples/standard
kubectl apply -f ./kube-state-metrics

# Deploy Prometheus (for monitoring)
kubectl apply -f ./prometheus.yml

# Deploy Prometheus Alertmanager (for sending alerts when things go wrong)
kubectl apply -f ./alertmanager.yml

# Deploy Grafana (for data metric graphing)
kubectl apply -f ./grafana.yml
```

Once you've deployed all the above, you'll be able to access:

* [Prometheus](https://prometheus.io) via port 30000 of the Kubernetes node's IP address, e.g. [http://192.168.1.7:30000](http://192.168.1.7:30000)
* [Alertmanager](https://prometheus.io/docs/alerting/latest/alertmanager/) via port 31000 of the Kubernetes node's IP address, e.g. [http://192.168.1.7:31000](http://192.168.1.7:31000)
* [Grafana](https://grafana.com) via port 32000 of the Kubernetes node's IP address, e.g. [http://192.168.1.7:32000](http://192.168.1.7:32000)

When you log into Grafana for the first time, you'll need to log in with `admin` as both the username and password. It will then prompt you to set a new password for the `admin` user.

You'll then need to set up any Grafana dashboards you want to be able to view useful data.

The first dashboard that is worth importing is the [Node Exporter Full](https://grafana.com/grafana/dashboards/1860) dashboard, which gets you a bunch of hardware info about the computer running Kubernetes.
