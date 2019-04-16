######################
# Create The Cluster #
######################

# Tested with minikube v0.30

minikube start \
    --vm-driver virtualbox \
    --cpus 2 \
    --memory 10240

###############################
# Install Ingress and Storage #
###############################

minikube addons enable ingress

minikube addons enable storage-provisioner

minikube addons enable default-storageclass

##################
# Install Tiller #
##################

kubectl create \
    -f https://raw.githubusercontent.com/vfarcic/k8s-specs/master/helm/tiller-rbac.yml \
    --record --save-config

helm init --service-account tiller

kubectl -n kube-system \
    rollout status deploy tiller-deploy

##################
# Metrics Server #
##################

minikube addons enable metrics-server

kubectl -n kube-system rollout status deployment metrics-server

##################
# Get Cluster IP #
##################

export LB_IP=$(minikube ip)
    
#######################
# Destroy the cluster #
#######################

kubectl create secret docker-registry regcred --docker-server=index.docker.io/v1/ --docker-username=sikor1111 --docker-password=s --docker-email=itmaciej.sikora@gmail.com