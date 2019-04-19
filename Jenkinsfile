import java.text.SimpleDateFormat

pipeline {
  options {
    buildDiscarder logRotator(numToKeepStr: '5')
    disableConcurrentBuilds()
  }
  agent {
    kubernetes {
      cloud "kubernetes"
      label "kubernetes"
      serviceAccount "qa-infrastructure-jenkins"
      yamlFile "KubernetesPod.yaml"
    }      
  }
  environment {
  }
  stages {
    stage("build") {
      steps {
        container('docker'){
            sh "ls -all"
            // sh "docker image build -t ${sikor1111}/dotnet-demo:beta"
            // withCredentials([usernamePassword(
            //     credentialsId: "docker",
            //     usernameVariavble: "USER",
            //     passwordVariable: "PASS"
            // )]){
            //     sh "docker login -u '$USER' -p '$PASS'"
            // }
            // sh "docker image push sikor1111/dotnet-demo:beta"
        }
      }
    }
  }
}
