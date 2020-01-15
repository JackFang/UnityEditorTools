使用Mesh处理Image RawImage:
运用场景：
当需要对图片进行圆角处理时，使用传统的2DMask UI遮罩，增加DrawCall，效率低下
使用处理后的FilletImage FilletRawImage,可减少DrawCall，无需遮罩。