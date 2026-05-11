pipeline {
    // 指定在Windows Agent上运行
    agent {
        label 'unity-windows'  // 改成你在节点配置中设置的标签名
    }

    // 参数定义
    parameters {
        choice(
            name: 'BUILD_TARGET',
            choices: ['Android', 'Windows'],
            description: '选择要打包的平台'
        )
        string(
            name: 'BRANCH',
            defaultValue: 'main',
            description: 'Git分支名'
        )
        booleanParam(
            name: 'DEVELOPMENT_BUILD',
            defaultValue: false,
            description: '是否构建Development Build'
        )
    }

    // 环境变量配置 - 根据你的实际路径修改这里！！！
    environment {
        // Unity编辑器路径 - 根据你Windows机器上实际安装路径修改
        UNITY_PATH_ANDROID = 'G:\\unity\\Editor\\Unity.exe'
        UNITY_PATH_WINDOWS = 'G:\\unity\\Editor\\Unity.exe'
        
        // Jenkins工作区中的项目路径（一般不需要改）
        PROJECT_PATH = "${WORKSPACE}\\UnityProject"
        
        // 构建输出目录
        BUILD_OUTPUT_DIR = "${WORKSPACE}\\Builds"
    }

    stages {
        // 阶段1：清理并拉取代码
        stage('Checkout') {
            steps {
                echo "正在从Git拉取代码，分支：${params.BRANCH}"
                checkout scm
                echo "代码拉取完成，工作目录：${env.WORKSPACE}"
            }
        }

        // 阶段2：执行Unity打包
        stage('Unity Build') {
            steps {
                script {
                    // 根据参数选择不同平台的打包命令
                    if (params.BUILD_TARGET == 'Android') {
                        echo "开始Android打包..."
                        bat """
                            "${env.UNITY_PATH_ANDROID}" ^
                            -batchmode ^
                            -nographics ^
                            -quit ^
                            -projectPath "${env.PROJECT_PATH}" ^
                            -executeMethod JenkinsBuild.BuildAndroid ^
                            -logFile "${env.BUILD_OUTPUT_DIR}\\build.log"
                        """
                        echo "Android打包完成"
                    } 
                    else if (params.BUILD_TARGET == 'Windows') {
                        echo "开始Windows打包..."
                        bat """
                            "${env.UNITY_PATH_WINDOWS}" ^
                            -batchmode ^
                            -nographics ^
                            -quit ^
                            -projectPath "${env.PROJECT_PATH}" ^
                            -executeMethod JenkinsBuild.BuildWindows ^
                            -logFile "${env.BUILD_OUTPUT_DIR}\\build.log"
                        """
                        echo "Windows打包完成"
                    }
                }
            }
        }

        // 阶段3：归档产物（保存安装包到Jenkins）
        stage('Archive Artifacts') {
            steps {
                script {
                    if (params.BUILD_TARGET == 'Android') {
                        archiveArtifacts artifacts: 'Builds/Android/*.apk', fingerprint: true
                    } else if (params.BUILD_TARGET == 'Windows') {
                        archiveArtifacts artifacts: 'Builds/Windows/*.exe, Builds/Windows/*.dll', fingerprint: true
                    }
                }
            }
        }
    }

    // 无论成功或失败，都执行的收尾操作
    post {
        always {
            echo "构建流程结束，状态：${currentBuild.currentResult}"
        }
        success {
            echo "打包成功！产物已归档，可在此次构建的Artifacts中下载"
        }
        failure {
            echo "打包失败，请检查Unity日志：${env.BUILD_OUTPUT_DIR}\\build.log"
        }
    }
}
