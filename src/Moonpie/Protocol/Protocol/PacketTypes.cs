﻿#region Copyright
// Moonpie
// 
// Copyright (c) 2022 Stay
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

namespace Moonpie.Protocol.Protocol;

public class PacketTypes
{
    public enum C2S
    {
        HANDSHAKING_HANDSHAKE,
        STATUS_PING,
        STATUS_REQUEST,
        LOGIN_LOGIN_START,
        LOGIN_ENCRYPTION_RESPONSE,
        LOGIN_PLUGIN_RESPONSE,
        PLAY_TELEPORT_CONFIRM,
        PLAY_QUERY_BLOCK_NBT,
        PLAY_SET_DIFFICULTY,
        PLAY_CHAT_MESSAGE,
        PLAY_CLIENT_ACTION,
        PLAY_CLIENT_SETTINGS,
        PLAY_AUTOCOMPLETIONS,
        PLAY_CONTAINER_ACTION_STATUS,
        PLAY_CONTAINER_CLICK_BUTTON,
        PLAY_CONTAINER_SLOT_CLICK,
        PLAY_CONTAINER_CLOSE,
        PLAY_PLUGIN_MESSAGE,
        PLAY_EDIT_BOOK,
        PLAY_ENTITY_NBT_REQUEST,
        PLAY_INTERACT_ENTITY,
        PLAY_HEARTBEAT,
        PLAY_LOCK_DIFFICULTY,
        PLAY_POSITION,
        PLAY_POSITION_AND_ROTATION,
        PLAY_ROTATION,
        PLAY_VEHICLE_MOVE,
        PLAY_BOAT_STEER,
        PLAY_PICK_ITEM,
        PLAY_CRAFTING_RECIPE_REQUEST,
        PLAY_FLY_TOGGLE,
        PLAY_PLAYER_ACTION,
        PLAY_ENTITY_ACTION,
        PLAY_VEHICLE_STEER,
        PLAY_RECIPE_BOOK_STATE,
        PLAY_ANVIL_NAME_SET,
        PLAY_RESOURCE_PACK_STATUS,
        PLAY_ADVANCEMENT_TAB,
        PLAY_TRADE_SELECT,
        PLAY_BEACON_EFFECT_SET,
        PLAY_HOTBAR_SLOT_SET,
        PLAY_UPDATE_COMMAND_BLOCK,
        PLAY_ITEM_STACK_CREATE,
        PLAY_UPDATE_JIGSAW_BLOCK,
        PLAY_UPDATE_STRUCTURE_BLOCK,
        PLAY_SIGN_TEXT_SET,
        PLAY_ARM_SWING,
        PLAY_ENTITY_SPECTATE,
        PLAY_BLOCK_INTERACT,
        PLAY_ITEM_USE,
        PLAY_MINECART_COMMAND_BLOCK_SET,
        PLAY_GENERATE_STRUCTURE,
        PLAY_DISPLAYED_RECIPE_SET,
        PLAY_PLAYER_GROUND_CHANGE,
        PLAY_PREPARE_CRAFTING_GRID,
        PLAY_QUERY_ENTITY_NBT,
        PLAY_PONG
    }

    public enum S2C
    {
        STATUS_RESPONSE,
        STATUS_PONG,
        LOGIN_KICK,
        LOGIN_ENCRYPTION_REQUEST,
        LOGIN_LOGIN_SUCCESS,
        LOGIN_COMPRESSION_SET,
        LOGIN_PLUGIN_REQUEST,
        PLAY_MOB_SPAWN,
        PLAY_EXPERIENCE_ORB_SPAWN,
        PLAY_GLOBAL_ENTITY_SPAWN,
        PLAY_PAINTING_SPAWN,
        PLAY_PLAYER_ENTITY_SPAWN,
        PLAY_ENTITY_ANIMATION,
        PLAY_STATS_RESPONSE,
        PLAY_BLOCK_BREAK_ACK,
        PLAY_BLOCK_BREAK_ANIMATION,
        PLAY_BLOCK_ENTITY_META_DATA,
        PLAY_BLOCK_ACTION,
        PLAY_BLOCK_SET,
        PLAY_BOSS_BAR,
        PLAY_SERVER_DIFFICULTY,
        PLAY_CHAT_MESSAGE,
        PLAY_MASS_BLOCK_SET,
        PLAY_AUTOCOMPLETIONS,
        PLAY_DECLARE_COMMANDS,
        PLAY_CONTAINER_ACTION_STATUS,
        PLAY_CONTAINER_CLOSE,
        PLAY_CONTAINER_ITEMS_SET,
        PLAY_CONTAINER_PROPERTY_SET,
        PLAY_CONTAINER_ITEM_SET,
        PLAY_ITEM_COOLDOWN_SET,
        PLAY_PLUGIN_MESSAGE,
        PLAY_NAMED_SOUND_EVENT,
        PLAY_KICK,
        PLAY_ENTITY_STATUS,
        PLAY_EXPLOSION,
        PLAY_CHUNK_UNLOAD,
        PLAY_GAME_EVENT,
        PLAY_HORSE_CONTAINER_OPEN,
        PLAY_HEARTBEAT,
        PLAY_CHUNK_DATA,
        PLAY_WORLD_EVENT,
        PLAY_PARTICLE,
        PLAY_CHUNK_LIGHT_DATA,
        PLAY_JOIN_GAME,
        PLAY_MAP_DATA,
        PLAY_VILLAGER_TRADES,
        PLAY_ENTITY_MOVE_AND_ROTATE,
        PLAY_ENTITY_ROTATION,
        PLAY_ENTITY_RELATIVE_MOVE,
        PLAY_VEHICLE_MOVE,
        PLAY_BOOK_OPEN,
        PLAY_CONTAINER_OPEN,
        PLAY_SIGN_EDITOR_OPEN,
        PLAY_CRAFTING_RECIPE_RESPONSE,
        PLAY_PLAYER_ABILITIES,
        PLAY_COMBAT_EVENT,
        PLAY_COMBAT_EVENT_END,
        PLAY_COMBAT_EVENT_ENTER,
        PLAY_COMBAT_EVENT_KILL,
        PLAY_TAB_LIST_DATA,
        PLAY_PLAYER_FACE,
        PLAY_POSITION_AND_ROTATION,
        PLAY_UNLOCK_RECIPES,
        PLAY_ENTITY_DESTROY,
        PLAY_ENTITY_STATUS_EFFECT_REMOVE,
        PLAY_RESOURCEPACK_REQUEST,
        PLAY_RESPAWN,
        PLAY_ENTITY_HEAD_ROTATION,
        PLAY_SELECT_ADVANCEMENT_TAB,
        PLAY_WORLD_BORDER,
        PLAY_WORLD_BORDER_INITIALIZE,
        PLAY_CENTER_SET_WORLD_BORDER_,
        PLAY_WORLD_BORDER_LERP_SIZE,
        PLAY_WORLD_BORDER_SIZE,
        PLAY_WORLD_BORDER_SET_WARN_TIME,
        PLAY_WORLD_BORDER_SET_WARN_BLOCKS,
        PLAY_CAMERA,
        PLAY_HOTBAR_SLOT_SET,
        PLAY_CHUNK_CENTER_SET,
        PLAY_OBJECTIVE_POSITION_SET,
        PLAY_ENTITY_METADATA,
        PLAY_ENTITY_ATTACH,
        PLAY_ENTITY_VELOCITY,
        PLAY_ENTITY_EQUIPMENT,
        PLAY_EXPERIENCE_SET,
        PLAY_HEALTH_SET,
        PLAY_SCOREBOARD_OBJECTIVE,
        PLAY_ENTITY_PASSENGER_SET,
        PLAY_TEAMS,
        PLAY_UPDATE_SCORE,
        PLAY_COMPASS_POSITION_SET,
        PLAY_WORLD_TIME_SET,
        PLAY_ENTITY_SOUND_EVENT,
        PLAY_SOUND_EVENT,
        PLAY_STOP_SOUND,
        PLAY_TAB_LIST_TEXT_SET,
        PLAY_NBT_QUERY_RESPONSE,
        PLAY_ENTITY_COLLECT_ANIMATION,
        PLAY_ENTITY_TELEPORT,
        PLAY_ADVANCEMENTS,
        PLAY_ENTITY_EFFECT_ATTRIBUTES,
        PLAY_ENTITY_STATUS_EFFECT,
        PLAY_DECLARE_RECIPES,
        PLAY_TAGS,
        PLAY_BED_USE,
        PLAY_VIEW_DISTANCE_SET,
        PLAY_MASS_CHUNK_DATA,
        PLAY_SIGN_TEXT_SET,
        PLAY_STATISTICS,
        PLAY_ENTITY_OBJECT_SPAWN,
        PLAY_TITLE,
        PLAY_TITLE_CLEAR,
        PLAY_HOTBAR_TEXT_SET,
        PLAY_TITLE_SUBTITLE_SET,
        PLAY_TITLE_SET,
        PLAY_TITLE_TIMES_SET,
        PLAY_EMPTY_ENTITY_MOVE,
        PLAY_COMPRESSION_SET,
        PLAY_ADVANCEMENT_PROGRESS,
        PLAY_VIBRATION_SIGNAL,
        PLAY_PING,
        PLAY_SIMULATION_DISTANCE,
    }
}