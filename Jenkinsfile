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
    image = "vfarcic/go-demo-5"
    project = "go-demo-5"
    domain = "34.210.146.155.nip.io"
    cmAddr = "cm.34.210.146.155.nip.io"
  }
  stages {
    stage("build") {
      steps {
        // container("golang") {
        //   script {
        //     currentBuild.displayName = new SimpleDateFormat("yy.MM.dd").format(new Date()) + "-${env.BUILD_NUMBER}"
        //   }
        //   k8sBuildGolang("go-demo")
        // }
         container(name: 'kaniko', shell: '/busybox/sh')
          withEnv(['PATH+EXTRA=/busybox:/kaniko']) {                
    sh '''#!/busybox/sh/kaniko/executor -f `pwd`/Dockerfile -c `pwd` --insecure --skip-tls-verify --cache=true --destination=index.docker.io/v1/sikor1111/mastering-docker:beta'''
        }
      }
      }
    }
    // stage("func-test") {
    //   steps {
    //     container("helm") {
    //       k8sUpgradeBeta(project, domain, "--set replicaCount=2 --set dbReplicaCount=1")
    //     }
    //     container("kubectl") {
    //       k8sRolloutBeta(project)
    //     }
    //     container("golang") {
    //       k8sFuncTestGolang(project, domain)
    //     }
    //   }
    //   post {
    //     always {
    //       container("helm") {
    //         k8sDeleteBeta(project)
    //       }
    //     }
    //   }
    // }
    stage("release") {
      when {
          branch "master"
      }
      steps {
        container("kaniko") {
          k8sPushImage(image, false)
        }
        container("helm") {
          k8sPushHelm(project, "", cmAddr, true, true)
        }
      }
    }
  }
}
