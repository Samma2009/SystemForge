(block) => {
    const X = Gen.valueToCode(block, 'X', javascript.Order.ATOMIC);
    const Y = Gen.valueToCode(block, 'Y', javascript.Order.ATOMIC);
    const Color = Gen.valueToCode(block, 'Color', javascript.Order.ATOMIC);

    const code = `fb_ptr[(${Y}) * (framebuffer->pitch / 4) + (${X})] = ${Color};\n`;
    return code;
}