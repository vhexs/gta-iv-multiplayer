// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MIVSDK;
using SharpDX;

namespace MIVServer
{
    public class PlayerCamera
    {
        private ServerPlayer player;

        public PlayerCamera(ServerPlayer player)
        {
            this.player = player;
        }

        public Vector3 Direction
        {
            set
            {
                var bpf = new BinaryPacketFormatter(Commands.Camera_setDirection);
                bpf.Add(value);
                player.connection.write(bpf.getBytes());
            }
        }

        public float FOV
        {
            set
            {
                var bpf = new BinaryPacketFormatter(Commands.Camera_setFOV);
                bpf.Add(value);
                player.connection.write(bpf.getBytes());
            }
        }

        public Vector3 Orientation
        {
            set
            {
                var bpf = new BinaryPacketFormatter(Commands.Camera_setOrientation);
                bpf.Add(value);
                player.connection.write(bpf.getBytes());
            }
        }

        public Vector3 Position
        {
            set
            {
                var bpf = new BinaryPacketFormatter(Commands.Camera_setPosition);
                bpf.Add(value);
                player.connection.write(bpf.getBytes());
            }
        }

        public void LookAt(Vector3 position)
        {
            var bpf = new BinaryPacketFormatter(Commands.Camera_lookAt);
            bpf.Add(position);
            player.connection.write(bpf.getBytes());
        }

        public void Reset()
        {
            var bpf = new BinaryPacketFormatter(Commands.Camera_reset);
            player.connection.write(bpf.getBytes());
        }
    }
}