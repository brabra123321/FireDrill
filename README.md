# FireDrill
消防演练实训项目

## 一、项目文件夹规约

``` 
Assets
--Animations: 放各种unity里面创建的animation动画，和animator控制器
--Audios：放各种声音文件，比如普通的mp3，wav等，以及unity里面创建的audio mixer
--FBX:（名称沿用老师的包）放各种模型，连同它自己的material（材质）和texture（纹理）放在一起
  --模型名字：放这个模型的fbx模型，材质和贴图。
    --Animations：如果有骨骼动画，那么动画相关的文件放在这里而不是上面的Animations文件夹
--Materials：放各种材质（模型对应的就不用再放到这里了）
--Packages: 用来放asset store或者其他地方导进来的包，一般刚导入的时候不在这里面，记得手动转移进来。
--Prefabs：放一些不需要动态加载的预制体
  --UI：UI预制体的独立文件夹
  --Particles：粒子系统预制体的独立文件夹
--Resources: unity特殊名称文件夹，放需要在脚本中使用Resources.load加载的各种文件夹
  --UI
  --Textures
  --Prefabs
  --等等各种以上上层文件夹，需要的都可以在这里面再创建一份
--Scripts：脚本文件夹。
  --core：放比较核心和通用的类
  --Editor：unity特殊名称文件夹，放编辑器拓展的类
  --UI：UI控制相关的类
  --其他有需要的可再创建单独的文件夹
--Scenes: 放各种场景文件
--Shaders：各种shader文件。这个项目应该不是很多，不用下层文件夹。
--StreamingAssets：unity特殊名称文件夹，用来放默认全部打包的东西（比如json文件，assetbundle之类）
  --ABS：assetbundle的单独文件夹
--Textures：放置各种杂七杂八贴图文件。UI和模型对应的就不用再放在这个文件夹下了。
--UI：放UI相关的贴图等。
 --Fonts：放字体文件。
```

## 二、其他注意事项

想到一出写一出，捡自己工作相关的看看。或者都看看也行。

1. 预制体等（比如粒子之类）unity内部的东西，顶层坐标务必归零。ui的预制体可以视情况不用归零。
2. ui的锚点一定要弄好。靠在哪个位置锚点就要选哪儿。
3. Canvas的Canvas Scalar要调Scale With Screen Size，然后分辨率就设置1920*1080，ScreenMatchMode设置Expand.
4. ui的脚本里面可以引用其他类或者GameObject，其他类里面*尽量*不要放UI类的引用或者UI的GameObject
5. 每一个自己创建的脚本，外层一定要套namespace 项目名。比如这个项目就是 namespace Firedrill。
6. 这一次不是一个人写代码，所以每一个类要记得注释。

