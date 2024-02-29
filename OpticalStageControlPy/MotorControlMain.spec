# -*- mode: python ; coding: utf-8 -*-


a = Analysis(
    ['MotorControlMain.py'],
    pathex=[],
    binaries=[],
    datas=[('yukkuri_mini.ico', '.')],
    hiddenimports=[],
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=[],
    noarchive=False,
)
pyz = PYZ(a.pure)

exe = EXE(
    pyz,
    a.scripts,
    a.binaries,
    a.datas,
    [],
    name='OpticalStageControl',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    upx_exclude=[],
    runtime_tmpdir=None,
    console=False,
    disable_windowed_traceback=False,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
)

import shutil
shutil.copyfile('yukkuri_mini.ico', '{0}/yukkuri_mini.ico'.format(DISTPATH))
shutil.copytree('./NanoOpticalStage', '{0}/ArduinoNanoFirmware'.format(DISTPATH))