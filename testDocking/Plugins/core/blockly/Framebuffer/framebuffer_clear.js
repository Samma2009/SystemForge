(block) => {
    const Color = Gen.valueToCode(block, 'value', javascript.Order.ATOMIC);

    const code = `for (size_t i = 0; i < framebuffer->width * framebuffer->height; i++) fb_ptr[i] = ${Color};\n`;
    return code;
}