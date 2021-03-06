# envsubst Dockerfile

This image will process a filename which is passed as an argument and substitute $FOO placeholders with ENVIRONMENT VARIABLE values. A new file of the same name is written to the `/processed` directory.

This can be useful when running on Kubernetes and you wish to update secret placeholders in config files.

This image can run as an init-container after mounting a configmap into `/workdir`.  Because config map files are readonly you'll also need to mount an `emptyDir: {}` volume to the init-container `/processed` folder as well as in the main pod container where you wish your new config to be mounted.

An example:

```yaml
[...]
spec:
  replicas: 1
  template:
    spec:
      containers:
      - name: myContainer
        image: alpine
        volumeMounts:
        - name: config
          mountPath: /config
      initContainers:
      - name: init-config
        image: dibi/envsubst
        env:
        - name: mySecretVar
          valueFrom:
            secretKeyRef:
              name: mySecret
              key: key
        volumeMounts:
        - name: config
          mountPath: /processed
        - name: workdir
          mountPath: /workdir
      volumes:
      - name: workdir
        configMap:
          name: myConfigMap
      - name: config
        emptyDir: {}
```
