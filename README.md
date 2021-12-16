# Scott's Home Automation

A random collection of code and configuration I use in my home automation.

Most of the server code and configuration runs in [Kubernetes](https://kubernetes.io) running on a Raspberry Pi 4 8GB.

## Useful dev software

For doing local development, you'll probably want these pieces of software installed:

* [Docker Desktop](https://www.docker.com/get-started) for building and running standalone containers.
* [kubectl](https://kubernetes.io/docs/tasks/tools/) command-line tool for managing Kubernetes. Probably only need to manually install this if you don't install Docker Desktop.
* [Lens Desktop](https://k8slens.dev) for easy monitoring of Kubernetes clusters.

## Setting up the Raspberry Pi

Most of the containers are built for arm64 so you need a 64-bit OS running on the Raspberry Pi. As of writing this, Raspberry Pi OS doesn't yet fully support 64-bit installations using the [Raspberry Pi Imager](https://www.raspberrypi.com/software/) out-of-the-box but you can install any image using the Imager. So you'll need to grab the Imager app, and the latest 64-bit image from [here](https://downloads.raspberrypi.org/raspios_arm64/images/), and then select the "Use custom" option in the Imager app to load the image onto an SD Card.

Insert the freshly imaged SD Card into the Raspberry Pi and boot it up. Once you've completed the first boot setup and connected the Raspberry Pi to your local network, open up the `Raspberry Pi Configuration` window and set the following:

* Set the hostname to something unique and memorable.
* Enable the SSH Interface.
* Set the `Headless Resolution` to something reasonable like 1920x1080.

Once you've done the above (and probably been forced to reboot), you should be able to continue all the rest of the steps by SSH'ing into the Raspberry Pi from another computer, instead of needing to use the graphical interface. The SSH command will look something like `ssh pi@pi4-8gb.local` where `pi4-8gb` is the hostname you entered before.

Now that you've SSH'ed into the Raspberry Pi, you'll want to run the following commands to get everything that is required installed and updated:

```sh
# Update all system software
sudo apt update
sudo apt dist-upgrade -y

# Need to add some boot config, so edit the cmdline.txt config with:
sudo nano /boot/cmdline.txt
# And then add the following to the start of the line
cgroup_enable=memory cgroup_memory=1
# Then save and exit and reboot the Raspberry Pi with:
sudo reboot

# Next we'll be installing k3s for our Kubernetes distribution
curl -sfL https://get.k3s.io | sh -

# Confirm that k3s is installed and running
sudo k3s kubectl get node
```

You should now be good to go and start deploying containers to Kubernetes.

If you want to be able to manage the Kubernetes cluster from another computer using `kubectl`, then you'll need to do the following:

1. SSH into the Raspberry Pi.
2. Run: `sudo cat /etc/rancher/k3s/k3s.yaml`
3. Copy the entire contents of the file.
4. On the computer you want to have access, run: `mkdir -p ~/.kube && nano ~/.kube/config`
5. Paste the contents of the file.
6. Replace `127.0.0.1` with the static IP address of the Raspberry Pi.
7. Save and exit the file.
8. To make sure you now have access, run: `kubectl get node`
